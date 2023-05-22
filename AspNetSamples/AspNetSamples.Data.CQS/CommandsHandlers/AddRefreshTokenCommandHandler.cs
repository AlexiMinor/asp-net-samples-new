using AspNetSamples.Data.CQS.Commands;
using AspNetSamples.Data.Entities;
using MediatR;

namespace AspNetSamples.Data.CQS.CommandsHandlers;

public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand>
{
    private readonly NewsAggregatorContext _context;

    public AddRefreshTokenCommandHandler(NewsAggregatorContext context)
    {
        _context = context;
    }

    public async Task Handle(AddRefreshTokenCommand request, 
        CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(new RefreshToken()
        {
            UserId = request.UserId,
            Value = request.Value
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}