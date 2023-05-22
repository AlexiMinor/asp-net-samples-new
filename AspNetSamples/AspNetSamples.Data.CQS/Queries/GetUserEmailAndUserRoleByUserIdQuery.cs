using AspNetSamples.Core.DTOs;
using MediatR;

namespace AspNetSamples.Data.CQS.Queries;

public class GetUserRoleNameByUserIdQuery : IRequest<string>
{
    public int UserId { get; set; }
}