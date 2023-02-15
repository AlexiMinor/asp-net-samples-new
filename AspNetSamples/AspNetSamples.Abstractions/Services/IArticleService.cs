using AspNetSamples.Data.Entities;

namespace AspNetSamples.Abstractions.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetArticlesWithSourceAsync();
        Task<Article> GetArticleByIdAsync(int id);
    }
}