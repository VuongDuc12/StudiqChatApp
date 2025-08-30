       
using Microsoft.EntityFrameworkCore;
using Ucm.Domain.Entities;
using Ucm.Domain.Entities.Chat;
using Ucm.Domain.IRepositories.Chat;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models.Chat;

namespace Ucm.Infrastructure.Repositories.Chat
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly AppDbContext _context;
        public FriendRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid fromUserId, Guid toUserId)
        {
            // Chỉ kiểm tra các request đang chờ (Status == 0)
            return await _context.FriendRequests.AnyAsync(x =>
                ((x.FromUserId == fromUserId && x.ToUserId == toUserId) ||
                 (x.FromUserId == toUserId && x.ToUserId == fromUserId))
                && x.Status == 0);
        }

        public async Task AddAsync(FriendRequest request)
        {
            var ef = new FriendRequestEf
            {
                Id = request.Id,
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                HandledAt = request.HandledAt
            };
            await _context.FriendRequests.AddAsync(ef);
            await _context.SaveChangesAsync();
        }

        public async Task<FriendRequest> AcceptAsync(Guid requestId)
        {
            var ef = await _context.FriendRequests.FirstOrDefaultAsync(x => x.Id == requestId);
            if (ef == null) return null;
            ef.Status = 1; // Accepted
            ef.HandledAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new FriendRequest
            {
                Id = ef.Id,
                FromUserId = ef.FromUserId,
                ToUserId = ef.ToUserId,
                Status = ef.Status,
                CreatedAt = ef.CreatedAt,
                HandledAt = ef.HandledAt
            };
        }

        public async Task<FriendRequest> RejectAsync(Guid requestId)
        {
            var ef = await _context.FriendRequests.FirstOrDefaultAsync(x => x.Id == requestId);
            if (ef == null) return null;
            ef.Status = 2; // Rejected
            ef.HandledAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new FriendRequest
            {
                Id = ef.Id,
                FromUserId = ef.FromUserId,
                ToUserId = ef.ToUserId,
                Status = ef.Status,
                CreatedAt = ef.CreatedAt,
                HandledAt = ef.HandledAt
            };
        }

        public async Task<List<FriendRequest>> GetReceivedRequestsAsync(Guid userId)
        {
            var list = await _context.FriendRequests
                .Include(x => x.FromUser)
                .Where(x => x.ToUserId == userId && x.Status == 0)
                .ToListAsync();
            return list.Select(ef => Common.Mappers.FriendRequestEntityEfMapper.ToEntity(ef)).ToList();
        }

        public async Task<List<FriendRequest>> GetSentRequestsAsync(Guid userId)
        {
            var list = await _context.FriendRequests
                .Include(x => x.ToUser)
                .Where(x => x.FromUserId == userId && x.Status == 0)
                .ToListAsync();
            return list.Select(ef => Common.Mappers.FriendRequestEntityEfMapper.ToEntity(ef)).ToList();
        }

         public async Task<FriendRequest?> GetByIdAsync(Guid id)
        {
            var ef = await _context.FriendRequests.Include(x => x.FromUser).Include(x => x.ToUser).FirstOrDefaultAsync(x => x.Id == id);
            if (ef == null) return null;
            return Common.Mappers.FriendRequestEntityEfMapper.ToEntity(ef);
        }
    }
}