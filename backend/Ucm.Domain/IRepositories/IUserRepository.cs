using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Shared.Results;

namespace Ucm.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<Result> CreateAsync(AppUser user, string password, string role);
        Task<List<string>> GetRolesAsync(Guid userId);
    }
}
