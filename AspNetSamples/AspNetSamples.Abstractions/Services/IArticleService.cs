using AspNetSamples.Data.Entities;

namespace AspNetSamples.Abstractions.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetArticlesWithSourceAsync();
        Task<Article> GetArticleByIdAsync(int id);
        Task<int> AddAsync(Article article);
        Task<int> GetTotalArticlesCountAsync();
        IQueryable<Article> GetArticlesWithSourceNoTrackingAsQueryable();
    }
}