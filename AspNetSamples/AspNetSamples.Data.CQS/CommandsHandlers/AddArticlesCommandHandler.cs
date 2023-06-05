using AspNetSamples.Data.CQS.Commands;
using AspNetSamples.Data.Entities;
using AutoMapper;
using MediatR;

namespace AspNetSamples.Data.CQS.CommandsHandlers;

public class AddArticlesCommandHandler : IRequestHandler<AddArticlesCommand>
{
    private readonly NewsAggregatorContext _context;
    private readonly IMapper _mapper;

    public AddArticlesCommandHandler(NewsAggregatorContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(AddArticlesCommand request, 
        CancellationToken cancellationToken)
    {
        var articles = request.Articles.Select(dto => _mapper.Map<Article>(dto)).ToList();
        await _context.Articles.AddRangeAsync(articles, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}