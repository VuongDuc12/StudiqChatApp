using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities.Chat;
using Ucm.Domain.IRepositories.Chat;

namespace Ucm.Application.Services.Chat
{
    public class ChatNotificationService
    {
        private readonly IChatNotificationRepository _repo;
        public ChatNotificationService(IChatNotificationRepository repo)
        {
            _repo = repo;
        }

        public Task<ChatNotification> AddAsync(ChatNotification notification)
            => _repo.AddAsync(notification);

        public Task<List<ChatNotification>> GetByUserIdAsync(Guid userId, ChatNotificationType? type = null, bool? isRead = null)
            => _repo.GetByUserIdAsync(userId, type.HasValue ? (int?)type.Value : null, isRead);

        public Task<ChatNotification?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task MarkAsReadAsync(Guid id)
            => _repo.MarkAsReadAsync(id);

        public Task<int> CountUnreadAsync(Guid userId)
            => _repo.CountUnreadAsync(userId);
    }
}
