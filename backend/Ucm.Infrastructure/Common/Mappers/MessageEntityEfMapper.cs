using Ucm.Domain.Entities.Chat;
using Ucm.Infrastructure.Data.Models.Chat;

namespace Ucm.Infrastructure.Common.Mappers
{
    public static class MessageEntityEfMapper
    {
        public static MessageEf ToEf(Message m)
        {
            if (m == null) return null;
            return new MessageEf
            {
                Id = m.Id == Guid.Empty ? Guid.NewGuid() : m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                Content = m.Content,
                MessageType = m.MessageType,
                CreatedAt = m.CreatedAt == default ? DateTime.UtcNow : m.CreatedAt,
                ReplyToMessageId = m.ReplyToMessageId,
                IsEdited = m.IsEdited,
                EditedAt = m.EditedAt
            };
        }

        public static Message ToEntity(MessageEf ef)
        {
            if (ef == null) return null;
            return new Message
            {
                Id = ef.Id,
                ConversationId = ef.ConversationId,
                SenderId = ef.SenderId,
                Content = ef.Content,
                MessageType = ef.MessageType,
                CreatedAt = ef.CreatedAt,
                ReplyToMessageId = ef.ReplyToMessageId,
                IsEdited = ef.IsEdited,
                EditedAt = ef.EditedAt
            };
        }
    }
}
