using MediatR;

namespace AspNetSamples.Data.CQS.Commands;

public class AddRefreshTokenCommand : IRequest
{
    public int UserId { get; set; }
    public Guid Value { get; set; }
}