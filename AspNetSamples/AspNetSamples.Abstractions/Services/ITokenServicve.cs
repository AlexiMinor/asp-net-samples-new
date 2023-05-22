using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Services;

//Should be part of JwtService but splitted to show CQS implementation
public interface ITokenService
{
    Task AddRefreshTokenAsync(int userId, Guid refreshToken);
    Task RevokeRefreshTokenAsync(Guid refreshToken);
    Task<TokenDto> RefreshTokenAsync(Guid refreshToken);
}