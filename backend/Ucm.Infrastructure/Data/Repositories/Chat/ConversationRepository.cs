using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Domain.IRepositories.Chat;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models.Chat;
using Ucm.Domain.Entities.Chat;
using Ucm.Infrastructure.Common.Mappers;

namespace Ucm.Infrastructure.Data.Repositories.Chat
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _db;
        public ConversationRepository(AppDbContext db) { _db = db; }

        public async Task AddMessageAsync(Message message)
        {
            var ef = MessageEntityEfMapper.ToEf(message);
            await _db.Messages.AddAsync(ef);
            await _db.SaveChangesAsync();
            // update generated id back
            message.Id = ef.Id;
            message.CreatedAt = ef.CreatedAt;
        }

        public async Task<Conversation?> GetByIdWithMembersAsync(Guid id)
        {
            var ef = await _db.Conversations
                .Include(c => c.Members)
                .ThenInclude(m => m.User)
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(50))
                .FirstOrDefaultAsync(c => c.Id == id);
            if (ef == null) return null;
            return ConversationEntityEfMapper.ToEntity(ef);
        }

        public async Task<List<Conversation>> GetConversationsForUserAsync(Guid userId)
        {
            var list = await _db.Conversations
                .Where(c => c.Members.Any(m => m.UserId == userId))
                .Include(c => c.Members)
                .ThenInclude(m => m.User)
                .ToListAsync();
            return list.Select(ConversationEntityEfMapper.ToEntity).ToList();
        }

        public async Task<List<Message>> GetMessagesAsync(Guid conversationId, int page, int pageSize)
        {
            var list = await _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.Sender)
                .ToListAsync();
            // return in ascending order for UI
            return list.OrderBy(m => m.CreatedAt).Select(MessageEntityEfMapper.ToEntity).ToList();
        }

        public async Task UpdateConversationAsync(Conversation conversation)
        {
            var ef = await _db.Conversations.FindAsync(conversation.Id);
            if (ef == null) return;
            // map preview / members relevant fields
            ef.LastMessageId = conversation.Messages?.OrderByDescending(m => m.CreatedAt).FirstOrDefault()?.Id ?? ef.LastMessageId;
            ef.LastMessagePreview = conversation.Messages?.OrderByDescending(m => m.CreatedAt).FirstOrDefault()?.Content ?? ef.LastMessagePreview;
            ef.LastMessageAt = conversation.Messages?.OrderByDescending(m => m.CreatedAt).FirstOrDefault()?.CreatedAt ?? ef.LastMessageAt;
            // update unread counts per member
            foreach (var mem in conversation.Members ?? new List<ConversationMember>())
            {
                var memEf = ef.Members.FirstOrDefault(m => m.UserId == mem.UserId);
                if (memEf != null)
                {
                    memEf.UnreadCount = mem.UnreadCount;
                    memEf.LastReadAt = mem.LastReadAt;
                }
            }
            await _db.SaveChangesAsync();
        }

        public async Task<Conversation> CreateConversationAsync(Conversation conversation)
        {
            var ef = new ConversationEf
            {
                Id = conversation.Id == Guid.Empty ? Guid.NewGuid() : conversation.Id,
                Type = conversation.Type,
                Name = conversation.Name,
                AvatarUrl = conversation.AvatarUrl,
                CreatedBy = conversation.CreatedBy,
                CreatedAt = conversation.CreatedAt == default ? DateTime.UtcNow : conversation.CreatedAt
            };

            // create member ef entries
            if (conversation.Members != null)
            {
                foreach (var m in conversation.Members)
                {
                    ef.Members.Add(new ConversationMemberEf
                    {
                        Id = m.Id == Guid.Empty ? Guid.NewGuid() : m.Id,
                        ConversationId = ef.Id,
                        UserId = m.UserId,
                        JoinedAt = m.JoinedAt == default ? DateTime.UtcNow : m.JoinedAt,
                        Role = m.Role,
                        UnreadCount = m.UnreadCount,
                        LastReadAt = m.LastReadAt
                    });
                }
            }

            await _db.Conversations.AddAsync(ef);
            await _db.SaveChangesAsync();

            // reload with members to return mapped entity
            var saved = await _db.Conversations
                .Include(c => c.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == ef.Id);
            return ConversationEntityEfMapper.ToEntity(saved!);
        }
    }
}
