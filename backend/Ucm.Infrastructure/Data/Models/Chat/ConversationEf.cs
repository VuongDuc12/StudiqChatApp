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
    public ICollection<ConversationMemberEf> Members { get; set; } = new List<ConversationMemberEf>();
    public ICollection<MessageEf> Messages { get; set; } = new List<MessageEf>();

    // preview fields for fast conversation list
    public Guid? LastMessageId { get; set; }
    public string? LastMessagePreview { get; set; }
    public DateTime? LastMessageAt { get; set; }
    }
}
