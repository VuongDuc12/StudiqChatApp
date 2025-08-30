using Ucm.Domain.Entities;
using Ucm.Domain.Entities.Chat;

namespace Ucm.Domain.IRepositories.Chat
{
    public interface IFriendRequestRepository
    {
        /// <summary>
        /// Kiểm tra xem đã tồn tại lời mời kết bạn giữa hai người dùng chưa.
        /// </summary>
        /// <param name="fromUserId">Id người gửi lời mời</param>
        /// <param name="toUserId">Id người nhận lời mời</param>
        /// <returns>True nếu đã tồn tại lời mời, False nếu chưa</returns>
    Task<bool> ExistsAsync(Guid fromUserId, Guid toUserId);

    /// <summary>
    /// Thêm mới một lời mời kết bạn vào hệ thống.
    /// </summary>
    /// <param name="request">Đối tượng FriendRequest cần thêm</param>
    Task AddAsync(FriendRequest request);

    /// <summary>
    /// Chấp nhận lời mời kết bạn (cập nhật trạng thái và trả về entity đã cập nhật)
    /// </summary>
    Task<FriendRequest> AcceptAsync(Guid requestId);

    /// <summary>
    /// Từ chối lời mời kết bạn (cập nhật trạng thái và trả về entity đã cập nhật)
    /// </summary>
    Task<FriendRequest> RejectAsync(Guid requestId);

    /// <summary>
    /// Lấy danh sách lời mời kết bạn gửi đến user
    /// </summary>
    Task<List<FriendRequest>> GetReceivedRequestsAsync(Guid userId);

    /// <summary>
    /// Lấy danh sách lời mời kết bạn đã gửi bởi user
    /// </summary>
    Task<List<FriendRequest>> GetSentRequestsAsync(Guid userId);
        /// <summary>
        /// Lấy chi tiết một lời mời kết bạn theo Id
        /// </summary>
        Task<FriendRequest?> GetByIdAsync(Guid id);
    }
}