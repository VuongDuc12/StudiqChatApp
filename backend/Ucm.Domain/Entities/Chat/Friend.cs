using System;

namespace Ucm.Domain.Entities.Chat
{
    public class Friend
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public Guid FriendId { get; set; }
        public AppUser FriendUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
