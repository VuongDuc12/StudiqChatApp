using Ucm.Domain.Entities;
using Ucm.Domain.Entities.Chat;

namespace Ucm.Domain.IRepositories.Chat
{
    public interface IFriendRepository
    {
        /// <summary>
        /// Lấy danh sách bạn bè của một người dùng.
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns>Danh sách Friend</returns>
        Task<List<Friend>> GetFriendsAsync(Guid userId);

        /// <summary>
        /// Thêm mới một quan hệ bạn bè vào hệ thống.
        /// </summary>
        /// <param name="friend">Đối tượng Friend cần thêm</param>
        Task AddAsync(Friend friend);

        /// <summary>
        /// Kiểm tra xem hai người dùng đã là bạn bè chưa.
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <param name="friendId">Id bạn bè</param>
        /// <returns>True nếu đã là bạn bè, False nếu chưa</returns>
        Task<bool> AreFriendsAsync(Guid userId, Guid friendId);

    /// <summary>
    /// Xóa quan hệ bạn bè giữa hai người dùng.
    /// </summary>
    /// <param name="userId">Id người dùng</param>
    /// <param name="friendId">Id bạn bè</param>
    Task RemoveAsync(Guid userId, Guid friendId);

    /// <summary>
    /// Tìm kiếm người dùng để gửi kết bạn (theo tên hoặc email, loại trừ bản thân và bạn bè đã có).
    /// </summary>
    /// <param name="keyword">Tên hoặc email</param>
    /// <param name="excludeUserId">Id người đang tìm kiếm (không trả về chính mình)</param>
    /// <returns>Danh sách AppUser phù hợp</returns>
    Task<List<AppUser>> SearchUsersAsync(string keyword, Guid excludeUserId);
    }
}