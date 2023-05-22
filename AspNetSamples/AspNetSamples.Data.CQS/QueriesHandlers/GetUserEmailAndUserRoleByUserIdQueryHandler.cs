using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.CQS.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Data.CQS.QueriesHandlers;

public class GetUserRoleNameByUserIdQueryHandler : IRequestHandler<GetUserRoleNameByUserIdQuery, string>
{
    private readonly NewsAggregatorContext _context;
    //private readonly IMapper _mapper;

    public GetUserRoleNameByUserIdQueryHandler(NewsAggregatorContext context, IMapper mapper)
    {
        _context = context;
        //_mapper = mapper;
    }

    public async Task<string> Handle(GetUserRoleNameByUserIdQuery request, CancellationToken cancellationToken)
    {
        var roleName = (await _context.Users
            .AsNoTracking()
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Id.Equals(request.UserId), 
                cancellationToken: cancellationToken))?.Role.Name;
        if (roleName != null)
        {
            return roleName;
        }
        else
        {
            throw new ArgumentException("Incorrect User Id", nameof(request.UserId));
        }
    }
}