using Microsoft.AspNetCore.Identity;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Data.Models;
using Ucm.Shared.Results;

namespace Ucm.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUserEF> _userManager;

        public UserRepository(UserManager<AppUserEF> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            var entity = await _userManager.FindByNameAsync(username);
            return entity == null ? null : MapToDomain(entity);
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            var entity = await _userManager.FindByIdAsync(id.ToString());
            return entity == null ? null : MapToDomain(entity);
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            var efUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return efUser != null && await _userManager.CheckPasswordAsync(efUser, password);
        }

        public async Task<Result> CreateAsync(AppUser user, string password, string role)
        {
            var efUser = new AppUserEF
            {
                Id = user.Id,
                UserName = user.Username,
                Email = user.Email,
                FullName = user.FullName
            };

            var result = await _userManager.CreateAsync(efUser, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Fail(errors);
            }

            // Gán Role
            var roleResult = await _userManager.AddToRoleAsync(efUser, role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Result.Fail($"User created but role assignment failed: {errors}");
            }

            return Result.Ok("User created successfully with role.");
        }

        public async Task<List<string>> GetRolesAsync(Guid userId)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            if (efUser == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(efUser);
            return roles.ToList();
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            var entities = _userManager.Users.ToList();
            return entities.Select(MapToDomain);
        }

        private AppUser MapToDomain(AppUserEF entity)
        {
            return new AppUser
            {
                Id = entity.Id,
                Username = entity.UserName,
                Email = entity.Email,
                FullName = entity.FullName
            };
        }
    }
}
