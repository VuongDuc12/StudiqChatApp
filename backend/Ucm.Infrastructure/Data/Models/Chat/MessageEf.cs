using System;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data.Models.Chat
{
    public class MessageEf
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public ConversationEf Conversation { get; set; }
        public Guid SenderId { get; set; }
        public AppUserEF Sender { get; set; }
        public string Content { get; set; }
        public int MessageType { get; set; } // 0=Text, 1=Image, 2=File, ...
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public Guid? ReplyToMessageId { get; set; }
        public MessageEf? ReplyToMessage { get; set; }
        public bool IsReading { get; set; } // true nếu đang được đọc
    }
}
