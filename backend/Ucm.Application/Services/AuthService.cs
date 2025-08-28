using System;
using System.Threading.Tasks;
using Ucm.Application.DTOs.Auth;
using Ucm.Application.IServices;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Shared.Results;

namespace Ucm.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
       
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepo,
          
            ITokenService tokenService)
        {
            _userRepo = userRepo;
           
            _tokenService = tokenService;
        }

        // ---------- Đăng ký ----------
        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
          
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName
                
            };

            return await _userRepo.CreateAsync(user, request.Password, request.Role);
        }

        // ---------- Đăng nhập ----------
        public async Task<Result<string>> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByUsernameAsync(request.Username);
            if (user == null)
                return Result<string>.Fail("User not found");

            var isValid = await _userRepo.CheckPasswordAsync(user, request.Password);
            if (!isValid)
                return Result<string>.Fail("Invalid credentials");

            // Lấy roles
            var roles = await _userRepo.GetRolesAsync(user.Id);

            // Tạo JWT có chứa claim role
            var token = _tokenService.GenerateToken(user, roles);

            return Result<string>.Ok(token, "Login successful");
        }

        public async Task<AppUser> GetUserAsync(string username)
        {
            var user = await _userRepo.GetByUsernameAsync(username);
            return user;
        }

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            return await _userRepo.GetRolesAsync(userId);
        }
    }
}
