using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Services
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetCommentsByArticleIdAsync(int articleId);
        
    }
}