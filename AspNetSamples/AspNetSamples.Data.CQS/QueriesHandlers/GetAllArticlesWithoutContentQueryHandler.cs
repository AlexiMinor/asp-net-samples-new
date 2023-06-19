using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.CQS.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Data.CQS.QueriesHandlers;

public class GetAllArticlesWithoutContentQueryHandler : IRequestHandler<GetAllArticlesWithoutContentQuery, List<ArticleDto>>
{
    private readonly NewsAggregatorContext _context;
    private readonly IMapper _mapper;

    public GetAllArticlesWithoutContentQueryHandler(NewsAggregatorContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ArticleDto>> Handle(GetAllArticlesWithoutContentQuery request, CancellationToken cancellationToken)
    {
        var articleDtos= await _context.Articles
            .AsNoTracking()
            .Where(article => string.IsNullOrEmpty(article.FullText))
            .Select(source => _mapper.Map<ArticleDto>(source))
            .ToListAsync(cancellationToken: cancellationToken);

        return articleDtos;
    }
}