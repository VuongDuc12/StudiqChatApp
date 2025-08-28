using System;
using System.Collections.Generic;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data.Models.Chat
{
    public class ConversationEf
    {
        public Guid Id { get; set; }
        public int Type { get; set; } // 0=Direct, 1=Group
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public Guid CreatedBy { get; set; }
        public AppUserEF Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ConversationMemberEf> Members { get; set; }
        public ICollection<MessageEf> Messages { get; set; }
    }
}
