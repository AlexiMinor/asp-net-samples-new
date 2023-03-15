using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Data.Repositories;

public interface IArticleRepository
{
    public Task<ArticleDto?> GetArticleByIdAsync(int id);

    public Task<List<ArticleDto>> GetArticlesAsync();

    public Task AddArticleAsync(ArticleDto dto);
    public Task AddArticlesAsync(IEnumerable<ArticleDto> dtos);

    public Task UpdateArticle(ArticleDto dto);
    public Task<int> CountAsync();
    public Task RemoveArticle(ArticleDto dto);
    public Task RemoveArticlesAsync(IEnumerable<ArticleDto> dtos);
    public Task<List<ArticleDto>> GetArticlesForPageAsync(int page, int pageSize);
}