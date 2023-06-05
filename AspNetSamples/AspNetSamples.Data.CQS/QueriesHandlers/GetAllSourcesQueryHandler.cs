using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.CQS.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Data.CQS.QueriesHandlers;

public class GetAllSourcesQueryHandler : IRequestHandler<GetAllSourcesQuery, List<SourceDto>>
{
    private readonly NewsAggregatorContext _context;
    private readonly IMapper _mapper;

    public GetAllSourcesQueryHandler(NewsAggregatorContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SourceDto>> Handle(GetAllSourcesQuery request, CancellationToken cancellationToken)
    {
        var sourceDtos = await _context.Sources
            .AsNoTracking()
            .Select(source => _mapper.Map<SourceDto>(source))
            .ToListAsync(cancellationToken: cancellationToken);

        return sourceDtos;
    }
}