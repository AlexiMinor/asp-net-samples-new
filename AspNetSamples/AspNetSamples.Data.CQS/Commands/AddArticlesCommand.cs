using AspNetSamples.Core.DTOs;
using MediatR;

namespace AspNetSamples.Data.CQS.Commands;

public class AddArticlesCommand : IRequest
{
    public IEnumerable<ArticleDto> Articles { get; set; }
}