using AspNetSamples.Data.CQS.Commands;
using AspNetSamples.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Data.CQS.CommandsHandlers;

public class RemoveRefreshTokenCommandHandler : IRequestHandler<RemoveRefreshTokenCommand>
{
    private readonly NewsAggregatorContext _context;

    public RemoveRefreshTokenCommandHandler(NewsAggregatorContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveRefreshTokenCommand request, 
        CancellationToken cancellationToken)
    {
        var rt = await _context.RefreshTokens.FirstOrDefaultAsync(token => token.Value.Equals(request.RefreshToken),
            cancellationToken);
        _context.RefreshTokens.Remove(rt);
        await _context.SaveChangesAsync(cancellationToken);
    }
}