using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities.Chat;

namespace Ucm.Domain.IRepositories.Chat
{
    public interface IChatNotificationRepository
    {
        Task<ChatNotification> AddAsync(ChatNotification notification);
        Task<List<ChatNotification>> GetByUserIdAsync(Guid userId, int? type = null, bool? isRead = null);
        Task<ChatNotification?> GetByIdAsync(Guid id);
        Task MarkAsReadAsync(Guid id);
        Task<int> CountUnreadAsync(Guid userId);
    }
}
