using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Data.Repositories;

public interface ICommentRepository
{
    public Task<CommentDto?> GetCommentByIdAsync(int id);

    public Task<List<CommentDto>> GetCommentsAsync();

    public Task AddCommentAsync(CommentDto dto);
    public Task AddCommentsAsync(IEnumerable<CommentDto> dtos);

    public Task UpdateComment(CommentDto dto);
    public Task<int> CountAsync();

    public Task RemoveComment(CommentDto dto);
    public Task RemoveCommentsAsync(IEnumerable<CommentDto> dtos);

    public Task<List<CommentDto>> GetCommentsByArticleIdAsync(int articleId);
}