using AspNetSamples.Data.CQS.Commands;
using AspNetSamples.Data.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Data.CQS.CommandsHandlers;

public class AddArticlesFullContentCommandHandler : IRequestHandler<AddArticlesFullContentCommand>
{
    private readonly NewsAggregatorContext _context;
    private readonly IMapper _mapper;

    public AddArticlesFullContentCommandHandler(NewsAggregatorContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(AddArticlesFullContentCommand command, 
        CancellationToken cancellationToken)
    {

        var articlesForUpdate = await _context.Articles.Where(article => command.Articles
            .Select(dto => dto.Id)
            .Contains(article.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var articlesDict = command.Articles.ToDictionary(dto => dto.Id, dto => dto.FullText);

        foreach (var article in articlesForUpdate)
        {
            article.FullText = articlesDict[article.Id];
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}