using System;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data.Models.Chat
{
    public class ChatNotificationEf
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUserEF? User { get; set; }
        public int Type { get; set; } // 0=FriendRequest, 1=Message, 2=Other
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
