using System;

namespace Ucm.Domain.Entities.Chat
{
    public class FriendRequest
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
    public AppUser? FromUser { get; set; }
        public Guid ToUserId { get; set; }
    public AppUser? ToUser { get; set; }
        public int Status { get; set; } // 0=Pending, 1=Accepted, 2=Rejected
        public DateTime CreatedAt { get; set; }
        public DateTime? HandledAt { get; set; }
    }
}
