using System.Linq;
using System.Linq;
using Ucm.Domain.Entities.Chat;
using Ucm.Infrastructure.Data.Models.Chat;
using Ucm.Domain.Entities;

namespace Ucm.Infrastructure.Common.Mappers
{
    public static class ConversationEntityEfMapper
    {
        public static Conversation ToEntity(ConversationEf ef)
        {
            if (ef == null) return null;
            var conv = new Conversation
            {
                Id = ef.Id,
                Type = ef.Type,
                Name = ef.Name,
                AvatarUrl = ef.AvatarUrl,
                CreatedBy = ef.CreatedBy,
                CreatedAt = ef.CreatedAt,
                Members = ef.Members?.Select(m => new ConversationMember
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    UserId = m.UserId,
                    User = m.User == null ? null : new AppUser { Id = m.User.Id, Username = m.User.UserName, FullName = m.User.FullName, Email = m.User.Email },
                    UnreadCount = m.UnreadCount,
                    LastReadAt = m.LastReadAt
                }).ToList(),
                Messages = ef.Messages?.OrderBy(m => m.CreatedAt).Select(m => new Message
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    Sender = m.Sender == null ? null : new AppUser { Id = m.Sender.Id, Username = m.Sender.UserName, FullName = m.Sender.FullName, Email = m.Sender.Email },
                    Content = m.Content,
                    MessageType = m.MessageType,
                    CreatedAt = m.CreatedAt,
                    ReplyToMessageId = m.ReplyToMessageId,
                    IsEdited = m.IsEdited,
                    EditedAt = m.EditedAt
                }).ToList()
            };
            return conv;
        }
    }
}
