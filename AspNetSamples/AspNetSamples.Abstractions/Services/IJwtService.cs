using System.Security.Claims;
using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Services
{
    public interface IJwtService
    {
        Task<string> GetTokenAsync(UserDto user);
        
    }
}