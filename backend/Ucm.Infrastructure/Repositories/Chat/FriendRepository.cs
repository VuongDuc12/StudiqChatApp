
using Microsoft.EntityFrameworkCore;
using Ucm.Domain.Entities;
using Ucm.Domain.Entities.Chat;
using Ucm.Domain.IRepositories.Chat;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;
using Ucm.Infrastructure.Data.Models.Chat;

namespace Ucm.Infrastructure.Repositories.Chat
{
    public class FriendRepository : IFriendRepository
    {
        private readonly AppDbContext _context;
        public FriendRepository(AppDbContext context)
        {
            _context = context;
        }
    public async Task<List<AppUser>> SearchUsersAsync(string keyword, Guid excludeUserId)
        {
            // Lấy danh sách bạn bè hiện tại
            var friendIds = await _context.Friends
                .Where(f => f.UserId == excludeUserId)
                .Select(f => f.FriendId)
                .ToListAsync();

            // Tìm kiếm user theo tên hoặc email, loại trừ bản thân và bạn bè đã có
            var users = await _context.Set<AppUserEF>()
                .Where(u => (u.FullName.Contains(keyword) || u.Email.Contains(keyword))
                            && u.Id != excludeUserId
                            && !friendIds.Contains(u.Id))
                .Select(u => new AppUser
                {
                    Id = u.Id,
                    Username = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email
                })
                .ToListAsync();
            return users;
        }
        public async Task<List<Friend>> GetFriendsAsync(Guid userId)
        {
            var friends = await _context.Friends
                .Include(f => f.User)
                .Include(f => f.FriendUser)
                .Where(f => f.UserId == userId)
                .ToListAsync();
            return friends.Select(f => new Friend
            {
                Id = f.Id,
                UserId = f.UserId,
                FriendId = f.FriendId,
                CreatedAt = f.CreatedAt,
                User = f.User == null ? null : new AppUser
                {
                    Id = f.User.Id,
                    Username = f.User.UserName,
                    FullName = f.User.FullName,
                    Email = f.User.Email
                },
                FriendUser = f.FriendUser == null ? null : new AppUser
                {
                    Id = f.FriendUser.Id,
                    Username = f.FriendUser.UserName,
                    FullName = f.FriendUser.FullName,
                    Email = f.FriendUser.Email
                }
            }).ToList();
        }

        public async Task AddAsync(Friend friend)
        {
            var ef = new FriendEf
            {
                Id = friend.Id,
                UserId = friend.UserId,
                FriendId = friend.FriendId,
                CreatedAt = friend.CreatedAt
            };
            await _context.Friends.AddAsync(ef);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AreFriendsAsync(Guid userId, Guid friendId)
        {
            return await _context.Friends.AnyAsync(f =>
                (f.UserId == userId && f.FriendId == friendId) ||
                (f.UserId == friendId && f.FriendId == userId));
        }

        public async Task RemoveAsync(Guid userId, Guid friendId)
        {
            var friends = await _context.Friends
                .Where(f => (f.UserId == userId && f.FriendId == friendId) ||
                            (f.UserId == friendId && f.FriendId == userId))
                .ToListAsync();
            if (friends.Any())
            {
                _context.Friends.RemoveRange(friends);
                await _context.SaveChangesAsync();
            }
        }
    }
}