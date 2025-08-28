using Ucm.Domain.Entities;
using Ucm.Domain.Entities.Chat;
using Ucm.Infrastructure.Data.Models.Chat;
using Ucm.Infrastructure.Common.Mappers;

namespace Ucm.Infrastructure.Common.Mappers
{
    public static class FriendRequestEntityEfMapper
    {
        public static FriendRequest ToEntity(FriendRequestEf ef)
        {
            if (ef == null) return null;
            return new FriendRequest
            {
                Id = ef.Id,
                FromUserId = ef.FromUserId,
                ToUserId = ef.ToUserId,
                Status = ef.Status,
                CreatedAt = ef.CreatedAt,
                HandledAt = ef.HandledAt,
                FromUser = AppUserEntityEfMapper.ToEntity(ef.FromUser),
                ToUser = AppUserEntityEfMapper.ToEntity(ef.ToUser)
            };
        }
    }
}
