using MediatR;

namespace AspNetSamples.Data.CQS.Commands;

public class RemoveRefreshTokenCommand : IRequest
{
    public Guid RefreshToken { get; set; }
}