using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.CQS.Commands;
using AspNetSamples.Data.CQS.Queries;
using AspNetSamples.Data.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AspNetSamples.Business;

public class TokenService : ITokenService
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public TokenService(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task AddRefreshTokenAsync(int userId, Guid refreshToken)
    {
        try
        {
            await _mediator.Send(new AddRefreshTokenCommand()
            {
                UserId = userId,
                Value = refreshToken
            });
        }
        catch (Exception ex)
        {
            //Logger<TokenService>.Error(ex);
            throw;
        }
        
    }

    public Task RevokeRefreshTokenAsync(Guid refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<TokenDto> RefreshTokenAsync(Guid refreshToken)
    {
        var user = await _mediator.Send(new GetUserByRefreshTokenQuery()
        {
            RefreshToken = refreshToken
        });

        if (user != null)
        {
            var jwt = await GenerateJwtToken(user);
            await _mediator.Send(new RemoveRefreshTokenCommand() { RefreshToken = refreshToken });

            var newRT = Guid.NewGuid();
            await _mediator.Send(new AddRefreshTokenCommand{UserId = user.Id, Value = refreshToken});

            return new TokenDto()
            {
                JwtToken = jwt,
                RefreshToken = newRT.ToString("D")
            };
        }
        throw new ArgumentException("RT not connected with User", nameof(refreshToken));

    }


    private async Task<string> GenerateJwtToken(UserDto user)
    {
        var role = await _mediator.Send(new GetUserRoleNameByUserIdQuery(){UserId = user.Id});
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
           new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
           new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("R"))
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:SecurityKey"]));

        var signIn = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpireInMinutes"])),
            signingCredentials: signIn);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}