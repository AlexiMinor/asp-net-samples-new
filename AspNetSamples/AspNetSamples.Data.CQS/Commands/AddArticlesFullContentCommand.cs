using AspNetSamples.Core.DTOs;
using MediatR;

namespace AspNetSamples.Data.CQS.Commands;

public class AddArticlesFullContentCommand : IRequest
{
    public IEnumerable<ArticleDto> Articles { get; set; }
}