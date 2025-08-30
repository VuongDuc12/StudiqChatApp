using System;

namespace Ucm.Application.DTOs.Chat
{
    public class CreateMessageRequest
    {
        public string Content { get; set; } = string.Empty;
        public int MessageType { get; set; } = 0;
        public Guid? ReplyToMessageId { get; set; }
    // optional: when creating/sending a direct (1:1) message, client may include recipientId
    public Guid? RecipientId { get; set; }
    // optional: when sending, client may leave ConversationId null to indicate server should create a new conversation
    public Guid? ConversationId { get; set; }
    }
}
