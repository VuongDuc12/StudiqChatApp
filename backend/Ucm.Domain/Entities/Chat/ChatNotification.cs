using System;

namespace Ucm.Domain.Entities.Chat
{
    public class ChatNotification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }
        public int Type { get; set; } // 0=FriendRequest, 1=Message, 2=Other
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
