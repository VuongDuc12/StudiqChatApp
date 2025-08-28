using System;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data.Models.Chat
{
    public class FriendEf
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUserEF User { get; set; }
        public Guid FriendId { get; set; }
        public AppUserEF FriendUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
