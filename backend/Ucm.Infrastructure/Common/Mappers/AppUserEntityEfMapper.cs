using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Common.Mappers
{
    public static class AppUserEntityEfMapper
    {
        public static AppUser ToEntity(AppUserEF ef)
        {
            if (ef == null) return null;
            return new AppUser
            {
                Id = ef.Id,
                Username = ef.UserName,
                FullName = ef.FullName,
                Email = ef.Email
            };
        }
    }
}
