using System;

namespace Ucm.Domain.Entities.Chat
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public Guid SenderId { get; set; }
        public AppUser Sender { get; set; }
        public string Content { get; set; }
        public int MessageType { get; set; } // 0=Text, 1=Image, 2=File, ...
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public Guid? ReplyToMessageId { get; set; }
        public Message? ReplyToMessage { get; set; }
        public bool IsReading { get; set; } // true nếu đang được đọc
    }
}
