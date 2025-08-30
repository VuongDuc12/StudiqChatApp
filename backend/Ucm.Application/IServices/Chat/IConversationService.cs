using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Application.DTOs.Chat;

namespace Ucm.Application.IServices.Chat
{
    public interface IConversationService
    {
        Task<List<ConversationDto>> GetConversationsForUserAsync(Guid userId);
        Task<List<MessageDto>> GetMessagesAsync(Guid conversationId, int page, int pageSize, Guid userId);
    Task<MessageDto> SendMessageAsync(Guid conversationId, Guid senderId, CreateMessageRequest req);
    Task<ConversationDto> CreateDirectConversationAsync(Guid userA, Guid userB);
    Task MarkConversationReadAsync(Guid conversationId, Guid userId, Guid? lastReadMessageId = null);
    }
}
