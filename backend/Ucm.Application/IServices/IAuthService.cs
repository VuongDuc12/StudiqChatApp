using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Application.DTOs.Auth;
using Ucm.Domain.Entities;
using Ucm.Shared.Results;

namespace Ucm.Application.IServices
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterRequest request);
        Task<Result<string>> LoginAsync(LoginRequest request);
        Task<AppUser> GetUserAsync(string username);
        Task<List<string>> GetUserRolesAsync(Guid userId);
    }
}
