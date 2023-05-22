using System.Security.Claims;
using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Services
{
    public interface IUserService
    {
        Task<bool> IsUserExistsAsync(string email);
        Task<UserDto?> RegisterAsync(string email, string password);
        Task<bool> IsPasswordCorrectAsync(string modelEmail, string modelPassword);
        Task<UserDto?> GetUserByEmailAsync(string modelEmail);
        Task<List<UserDto>> GetUsersAsync();

        Task<List<Claim>> GetUserClamsAsync(UserDto dto);
    }
}