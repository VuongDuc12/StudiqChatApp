using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Application.DTOs.Chat;
using Ucm.Application.IServices.Chat;
using Ucm.Domain.IRepositories.Chat;
using Ucm.Domain.Entities.Chat;

namespace Ucm.Application.Services.Chat
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _repo;
        public ConversationService(IConversationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ConversationDto>> GetConversationsForUserAsync(Guid userId)
        {
            var convs = await _repo.GetConversationsForUserAsync(userId);
            return convs.Select(c => new ConversationDto
            {
                Id = c.Id,
                Type = c.Type,
                Name = c.Name,
                AvatarUrl = c.AvatarUrl,
                CreatedAt = c.CreatedAt,
                Creator = c.Creator == null ? null : new SimpleUserDto { Id = c.Creator.Id, Username = c.Creator.Username, FullName = c.Creator.FullName },
                LastMessage = c.Messages?.OrderByDescending(m => m.CreatedAt).Select(m => new MessageDto {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    Sender = m.Sender == null ? null : new SimpleUserDto { Id = m.Sender.Id, Username = m.Sender.Username, FullName = m.Sender.FullName },
                    Content = m.Content,
                    MessageType = m.MessageType,
                    CreatedAt = m.CreatedAt,
                    ReplyToMessageId = m.ReplyToMessageId
                }).FirstOrDefault(),
                Members = c.Members?.Select(m => new SimpleUserDto { Id = m.UserId, Username = m.User?.Username, FullName = m.User?.FullName }).ToList() ?? new List<SimpleUserDto>(),
                UnreadCount = c.Members?.Where(m => m.UserId == userId).Select(m => m.Role == 0 ? 0 : 0).FirstOrDefault() ?? 0
            }).ToList();
        }

        public async Task<List<MessageDto>> GetMessagesAsync(Guid conversationId, int page, int pageSize, Guid userId)
        {
            var msgs = await _repo.GetMessagesAsync(conversationId, page, pageSize);
            return msgs.Select(m => new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                Sender = m.Sender == null ? null : new SimpleUserDto { Id = m.Sender.Id, Username = m.Sender.Username, FullName = m.Sender.FullName },
                Content = m.Content,
                MessageType = m.MessageType,
                CreatedAt = m.CreatedAt,
                ReplyToMessageId = m.ReplyToMessageId
            }).ToList();
        }

        public async Task<MessageDto> SendMessageAsync(Guid conversationId, Guid senderId, CreateMessageRequest req)
        {
            var conv = await _repo.GetByIdWithMembersAsync(conversationId);
            // if direct message type and conversation missing, throw to caller so they may create —
            // but also support creating here when messageType == 11 (1:1 chat) by throwing specialized exception
            if (conv == null)
            {
                if (req.MessageType == 11)
                {
                    throw new InvalidOperationException("ConversationNotFoundForDirectMessage");
                }
                throw new InvalidOperationException("Conversation not found");
            }
            if (!conv.Members.Any(m => m.UserId == senderId)) throw new UnauthorizedAccessException();
            var message = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                SenderId = senderId,
                Content = req.Content,
                MessageType = req.MessageType,
                CreatedAt = DateTime.UtcNow,
                ReplyToMessageId = req.ReplyToMessageId
            };

            // save message
            await _repo.AddMessageAsync(message);

            // attach to conversation in-memory
            conv.Messages = conv.Messages ?? new List<Message>();
            conv.Messages.Add(message);

            // update unread counters (and LastReadAt for sender)
            foreach (var member in conv.Members)
            {
                if (member.UserId == senderId)
                {
                    member.UnreadCount = 0;
                    member.LastReadAt = DateTime.UtcNow;
                }
                else
                {
                    member.UnreadCount += 1;
                }
            }

            await _repo.UpdateConversationAsync(conv);

            // attach sender info if available (so FE doesn't need extra API call)
            var senderUser = conv.Members.FirstOrDefault(m => m.UserId == senderId)?.User;

            return new MessageDto
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                Sender = senderUser == null ? null : new SimpleUserDto { Id = senderUser.Id, Username = senderUser.Username, FullName = senderUser.FullName, Email = senderUser.Email },
                Content = message.Content,
                MessageType = message.MessageType,
                CreatedAt = message.CreatedAt,
                ReplyToMessageId = message.ReplyToMessageId
            };
        }

        public async Task<ConversationDto> CreateDirectConversationAsync(Guid userA, Guid userB)
        {
            // create a direct conversation with two members
            var conv = new Conversation
            {
                Id = Guid.NewGuid(),
                Type = 0,
                Name = string.Empty,
                AvatarUrl = string.Empty,
                CreatedBy = userA,
                CreatedAt = DateTime.UtcNow,
                Members = new List<ConversationMember>
                {
                    new ConversationMember { Id = Guid.NewGuid(), ConversationId = Guid.Empty, UserId = userA, JoinedAt = DateTime.UtcNow, Role = 0, UnreadCount = 0 },
                    new ConversationMember { Id = Guid.NewGuid(), ConversationId = Guid.Empty, UserId = userB, JoinedAt = DateTime.UtcNow, Role = 0, UnreadCount = 0 }
                }
            };
            // ConversationId will be set by repository when creating
            var saved = await _repo.CreateConversationAsync(conv);
            return new ConversationDto
            {
                Id = saved.Id,
                Type = saved.Type,
                Name = saved.Name,
                AvatarUrl = saved.AvatarUrl,
                CreatedAt = saved.CreatedAt,
                Creator = saved.Creator == null ? null : new SimpleUserDto { Id = saved.Creator.Id, Username = saved.Creator.Username, FullName = saved.Creator.FullName },
                Members = saved.Members?.Select(m => new SimpleUserDto { Id = m.UserId, Username = m.User?.Username, FullName = m.User?.FullName }).ToList() ?? new List<SimpleUserDto>(),
                LastMessage = null,
                UnreadCount = 0
            };
        }

        public async Task MarkConversationReadAsync(Guid conversationId, Guid userId, Guid? lastReadMessageId = null)
        {
            var conv = await _repo.GetByIdWithMembersAsync(conversationId);
            if (conv == null) throw new InvalidOperationException("Conversation not found");
            var member = conv.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null) throw new UnauthorizedAccessException();
            member.UnreadCount = 0;
            member.LastReadAt = DateTime.UtcNow;
            await _repo.UpdateConversationAsync(conv);
        }
    }
}
