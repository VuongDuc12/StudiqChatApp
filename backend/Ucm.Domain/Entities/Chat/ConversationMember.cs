using System;

namespace Ucm.Domain.Entities.Chat
{
    public class ConversationMember
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
        public Guid UserId { get; set; }
    public AppUser? User { get; set; }
        public DateTime JoinedAt { get; set; }
        public int Role { get; set; } // 0=Member, 1=Admin, 2=Owner
    public int UnreadCount { get; set; }
    public DateTime? LastReadAt { get; set; }
    }
}
