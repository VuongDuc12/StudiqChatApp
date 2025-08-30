using System;

namespace Ucm.Domain.Entities.Chat
{
    public class ChatNotification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Người nhận
        public AppUser? User { get; set; }
        public Guid? FromUserId { get; set; } // Người gửi thông báo (có thể null)
        public AppUser? FromUser { get; set; }
        public ChatNotificationType Type { get; set; } // 0=FriendRequest, 1=Message, 2=Other
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public enum ChatNotificationType
{
    FriendRequest = 0,
    Message = 1,
    Other = 2
}
}
