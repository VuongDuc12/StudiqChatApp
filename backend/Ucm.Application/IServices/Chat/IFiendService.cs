namespace Ucm.Application.IServices.Chat
{
    public interface IFriendService
    {
        Task<bool> SendFriendRequestAsync(Guid fromUserId, Guid toUserId);
    }
}
