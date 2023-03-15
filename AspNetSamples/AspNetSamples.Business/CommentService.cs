using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Business;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CommentDto>> GetCommentsByArticleIdAsync(int articleId)
    {
        return await _unitOfWork.Comments.GetCommentsByArticleIdAsync(articleId);
    }
}