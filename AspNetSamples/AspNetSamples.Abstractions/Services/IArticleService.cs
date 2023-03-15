using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Abstractions.Services
{
    public interface IArticleService
    {
        //Task<Task<ArticleDto?>> GetArticleByIdAsync(int id);
        Task<ArticleDto?> GetArticleByIdWithSourceNameAsync(int id);
        Task AddAsync(ArticleDto article);
        Task<int> GetTotalArticlesCountAsync();
        //IQueryable<Article> GetArticlesWithSourceNoTrackingAsQueryable();
        Task<List<ArticleDto>> GetArticlesByPageAsync(int page, int pageSize);
        //Task<int> AddArticleWithNewSourceAsync();
    }
}