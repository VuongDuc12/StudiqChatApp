using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities.Chat;

namespace Ucm.Domain.IRepositories.Chat
{
    public interface IConversationRepository
    {
        Task<List<Conversation>> GetConversationsForUserAsync(Guid userId);
        Task<Conversation?> GetByIdWithMembersAsync(Guid id);
        Task<List<Message>> GetMessagesAsync(Guid conversationId, int page, int pageSize);
        Task AddMessageAsync(Message message);
        Task UpdateConversationAsync(Conversation conversation);
    Task<Conversation> CreateConversationAsync(Conversation conversation);
    }
}
