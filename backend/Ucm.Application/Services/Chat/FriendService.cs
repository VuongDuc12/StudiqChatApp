
using Ucm.Domain.Entities;
using Ucm.Domain.Entities.Chat;
using Ucm.Domain.IRepositories.Chat;

namespace Ucm.Application.Services.Chat
{
    public class FriendService
    {

        private readonly IFriendRepository _friendRepo;
        private readonly IFriendRequestRepository _friendRequestRepo;
        public FriendService(IFriendRepository friendRepo, IFriendRequestRepository friendRequestRepo)
        {
            _friendRepo = friendRepo;
            _friendRequestRepo = friendRequestRepo;
        }

        // Tìm kiếm user để gửi kết bạn
        public Task<List<AppUser>> SearchUsersAsync(string keyword, Guid currentUserId)
            => _friendRepo.SearchUsersAsync(keyword, currentUserId);

        // Gửi lời mời kết bạn
        public async Task<bool> SendFriendRequestAsync(Guid fromUserId, Guid toUserId)
        {
            if (await _friendRequestRepo.ExistsAsync(fromUserId, toUserId)) return false;
            var request = new FriendRequest
            {
                Id = Guid.NewGuid(),
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = 0,
                CreatedAt = DateTime.UtcNow
            };
            await _friendRequestRepo.AddAsync(request);
            return true;
        }

        // Chấp nhận lời mời kết bạn
        public async Task<bool> AcceptFriendRequestAsync(Guid requestId, Guid currentUserId)
        {
            var request = await _friendRequestRepo.AcceptAsync(requestId);
            if (request == null || request.Status != 1) return false;
            // Chỉ cho phép user nhận lời mời được accept
            if (request.ToUserId != currentUserId) return false;
            // Tạo quan hệ bạn bè 2 chiều
            await _friendRepo.AddAsync(new Friend
            {
                Id = Guid.NewGuid(),
                UserId = request.FromUserId,
                FriendId = request.ToUserId,
                CreatedAt = DateTime.UtcNow
            });
            await _friendRepo.AddAsync(new Friend
            {
                Id = Guid.NewGuid(),
                UserId = request.ToUserId,
                FriendId = request.FromUserId,
                CreatedAt = DateTime.UtcNow
            });
            return true;
        }

        // Xóa bạn bè
        public Task RemoveFriendAsync(Guid userId, Guid friendId)
            => _friendRepo.RemoveAsync(userId, friendId);

        // Lấy danh sách bạn bè
        public Task<List<Friend>> GetFriendsAsync(Guid userId)
            => _friendRepo.GetFriendsAsync(userId);

        // Lấy danh sách lời mời kết bạn đã gửi
        public Task<List<FriendRequest>> GetSentRequestsAsync(Guid userId)
            => _friendRequestRepo.GetSentRequestsAsync(userId);

        // Lấy danh sách lời mời kết bạn nhận được
        public Task<List<FriendRequest>> GetReceivedRequestsAsync(Guid userId)
            => _friendRequestRepo.GetReceivedRequestsAsync(userId);

        // Lấy chi tiết một lời mời kết bạn theo Id
        public Task<FriendRequest?> GetFriendRequestByIdAsync(Guid requestId)
            => _friendRequestRepo.GetByIdAsync(requestId);
    }
}