using AspNetSamples.Core.DTOs;
using MediatR;

namespace AspNetSamples.Data.CQS.Queries;

public class GetUserByRefreshTokenQuery : IRequest<UserDto>
{
    public Guid RefreshToken { get; set; }
}