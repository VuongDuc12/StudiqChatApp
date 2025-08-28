using System;
using System.Collections.Generic;

namespace Ucm.Domain.Entities.Chat
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public int Type { get; set; } // 0=Direct, 1=Group
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public Guid CreatedBy { get; set; }
        public AppUser Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ConversationMember> Members { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
