using AspNetSamples.Abstractions.Services;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Business
{
    public class ArticleService : IArticleService 
    {
        private readonly NewsAggregatorContext _dbContext;

        public ArticleService(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Article>> GetArticlesWithSourceAsync()
        {
            var articles = await _dbContext.Articles
                .Include(article => article.Source)
                .AsNoTracking()
                .ToListAsync();
            return articles;
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            var article = await _dbContext.Articles
                .FirstOrDefaultAsync(article1 => article1.Id == id);

            return article;
        }
    }
}