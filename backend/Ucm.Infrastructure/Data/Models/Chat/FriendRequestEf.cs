using System;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data.Models.Chat
{
    public class FriendRequestEf
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public AppUserEF FromUser { get; set; }
        public Guid ToUserId { get; set; }
        public AppUserEF ToUser { get; set; }
        public int Status { get; set; } // 0=Pending, 1=Accepted, 2=Rejected
        public DateTime CreatedAt { get; set; }
        public DateTime? HandledAt { get; set; }
    }
}
