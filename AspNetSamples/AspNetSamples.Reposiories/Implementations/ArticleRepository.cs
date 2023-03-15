using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsAggregatorContext _newsAggregatorContext;

        public ArticleRepository(NewsAggregatorContext newsAggregatorContext)
        {
            _newsAggregatorContext = newsAggregatorContext;
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(int id)
        {
            var article = await _newsAggregatorContext.Articles
                .Include(a=> a.Source)
                .AsNoTracking()
                .FirstOrDefaultAsync(article => article.Id == id);

            return 
                article == null 
                    ? null 
                    : Convert(article);
        }

        public async Task<List<ArticleDto>> GetArticlesAsync()
        {
            var articles = await _newsAggregatorContext.Articles
                .Include(a => a.Source)
                .AsNoTracking()
                .Select(article => Convert(article))
                .ToListAsync();

            return articles;
        }

        public async Task AddArticleAsync(ArticleDto dto)
        {
            var entity = Convert(dto);
            await _newsAggregatorContext.Articles.AddAsync(entity);

        }

        public async Task AddArticlesAsync(IEnumerable<ArticleDto> dtos)
        {
            var entities = dtos.Select(dto => Convert(dto)).ToList();
            await _newsAggregatorContext.Articles.AddRangeAsync(entities);
        }

        public async Task UpdateArticle(ArticleDto dto)
        {
            var ent = Convert(dto);

            var entForUpdate = await _newsAggregatorContext.Articles
                .FirstOrDefaultAsync(article => article.Id == ent.Id);

            if (entForUpdate!= null)
            {
                entForUpdate = ent;
            }

            await _newsAggregatorContext.SaveChangesAsync();

        }

        public async Task<int> CountAsync()
        {
            return await _newsAggregatorContext.Articles.CountAsync();
        }

        public async Task RemoveArticle(ArticleDto dto)
        {
            var ent = Convert(dto);
            _newsAggregatorContext.Articles.Remove(ent);
        }

        public async Task RemoveArticlesAsync(IEnumerable<ArticleDto> dtos)
        {
            var ents = dtos.Select(dto => Convert(dto)).ToList();
            _newsAggregatorContext.Articles.RemoveRange(ents);
        }

        public async Task<List<ArticleDto>> GetArticlesForPageAsync(int page, int pageSize)
        {
            return await _newsAggregatorContext.Articles
                .Include(article => article.Source)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(article => Convert(article))
                .ToListAsync();
        }
        
        private static ArticleDto Convert(Article article)
        {
            var dto = new ArticleDto
            {
                Id = article.Id,
                Rate = article.Rate,
                FullText = article.FullText,
                ShortDescription = article.ShortDescription,
                SourceId = article.SourceId,
                SourceName = article.Source.Name,
                Title = article.Title
            };

            return dto;
        }

        private static Article Convert(ArticleDto dto)
        {
            var article = new Article
            {
                Id = dto.Id,
                Rate = dto.Rate,
                FullText = dto.FullText,
                ShortDescription = dto.ShortDescription,
                SourceId = dto.SourceId,
                Title = dto.Title
            };

            return article;
        }
    }
}