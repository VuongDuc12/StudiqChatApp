using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ucm.Domain.Entities.Chat;
using Ucm.Domain.IRepositories.Chat;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models.Chat;

namespace Ucm.Infrastructure.Data.Repositories.Chat
{
    public class ChatNotificationRepository : IChatNotificationRepository
    {
        private readonly AppDbContext _db;
        public ChatNotificationRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ChatNotification> AddAsync(ChatNotification notification)
        {
            var ef = new ChatNotificationEf
            {
                Id = Guid.NewGuid(),
                UserId = notification.UserId,
                FromUserId = notification.FromUserId,
                Type = (int)notification.Type,
                Content = notification.Content,
                IsRead = notification.IsRead,
                CreatedAt = DateTime.UtcNow
            };
            _db.ChatNotifications.Add(ef);
            await _db.SaveChangesAsync();
            return new ChatNotification
            {
                Id = ef.Id,
                UserId = ef.UserId,
                FromUserId = ef.FromUserId,
                Type = (ChatNotificationType)ef.Type,
                Content = ef.Content,
                IsRead = ef.IsRead,
                CreatedAt = ef.CreatedAt
            };
        }

        public async Task<List<ChatNotification>> GetByUserIdAsync(Guid userId, int? type = null, bool? isRead = null)
        {
            var query = _db.ChatNotifications.AsQueryable();
            query = query.Where(n => n.UserId == userId);
            if (type.HasValue) query = query.Where(n => n.Type == type.Value);
            if (isRead.HasValue) query = query.Where(n => n.IsRead == isRead.Value);
            var list = await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return list.Select(ef => new ChatNotification
            {
                Id = ef.Id,
                UserId = ef.UserId,
                FromUserId = ef.FromUserId,
                Type = (ChatNotificationType)ef.Type,
                Content = ef.Content,
                IsRead = ef.IsRead,
                CreatedAt = ef.CreatedAt
            }).ToList();
        }

        public async Task<ChatNotification?> GetByIdAsync(Guid id)
        {
            var ef = await _db.ChatNotifications.FindAsync(id);
            if (ef == null) return null;
            return new ChatNotification
            {
                Id = ef.Id,
                UserId = ef.UserId,
                FromUserId = ef.FromUserId,
                Type = (ChatNotificationType)ef.Type,
                Content = ef.Content,
                IsRead = ef.IsRead,
                CreatedAt = ef.CreatedAt
            };
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var noti = await _db.ChatNotifications.FindAsync(id);
            if (noti != null && !noti.IsRead)
            {
                noti.IsRead = true;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<int> CountUnreadAsync(Guid userId)
        {
            return await _db.ChatNotifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}
