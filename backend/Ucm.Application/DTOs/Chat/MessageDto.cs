using System;

namespace Ucm.Application.DTOs.Chat
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public SimpleUserDto? Sender { get; set; }
        public string? Content { get; set; }
        public int MessageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ReplyToMessageId { get; set; }
    }
}
