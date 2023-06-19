using AspNetSamples.Core.DTOs;
using MediatR;

namespace AspNetSamples.Data.CQS.Queries;

public class GetAllArticlesWithoutContentQuery : IRequest<List<ArticleDto>>
{
    
}