using System;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data.Models.Chat
{
    public class ConversationMemberEf
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
    public ConversationEf? Conversation { get; set; }
        public Guid UserId { get; set; }
    public AppUserEF? User { get; set; }
        public DateTime JoinedAt { get; set; }
        public int Role { get; set; } // 0=Member, 1=Admin, 2=Owner
    }
}
