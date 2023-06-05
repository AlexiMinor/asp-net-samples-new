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
        Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(string partName);

        Task<List<ArticleDto>> AggregateArticlesDataFromRssSourceAsync(SourceDto source,
            CancellationToken cancellationToken);

        Task AddArticlesAsync(IEnumerable<ArticleDto> articles);
        //Task<string[]> GetContainsArticleUrlsBySourceAsync(int sourceId);
        Task<List<ArticleDto>> GetFullContentArticlesAsync(List<ArticleDto> articlesDataFromRss);

        Task<double?> GetArticleRateAsync(int articleId);
        Task<List<ArticleDto>> GetUnratedArticlesAsync();
        Task RateArticleAsync(int id, double? rate);
        Task AggregateArticlesDataFromRssAsync(CancellationToken cancellationToken);
    }
}