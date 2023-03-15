using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Repositories;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Business
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<List<ArticleDto>> GetArticlesWithSourceAsync()
        //{
        //    var articles = await _articleRepository.GetArticlesAsync();
        //    return articles;
        //}

        public async Task<int> GetTotalArticlesCountAsync()
        {
            var count = await _unitOfWork.Articles.CountAsync();
            return count;
        }

        //public IQueryable<Article> GetArticlesWithSourceNoTrackingAsQueryable()
        //{
        //    var articles = _dbContext.Articles
        //        .Include(article => article.Source)
        //        .AsNoTracking();
              
        //    return articles;
        //}

        public async Task<List<ArticleDto>> GetArticlesByPageAsync(int page, int pageSize)
        {
            var articlesByPage = await _unitOfWork.Articles.GetArticlesForPageAsync(page, pageSize);
            return articlesByPage;
        }

        public Task<int> AddArticleWithNewSourceAsync()
        {
            _unitOfWork.NewArticleRepository
                .FindBy(article => !string.IsNullOrEmpty(article.FullText),
                    article => article.Source,
                    article => article.Comments)
                .Select(article => new ArticleDto(){})
                .ToListAsync();

        }

        //public async Task<Task<ArticleDto?>> GetArticleByIdAsync(int id)
        //{
        //    var article = _articleRepository.GetArticleByIdAsync(id);

        //    return article;
        //}

        public async Task<ArticleDto?> GetArticleByIdWithSourceNameAsync(int id)
        {
            var article = await _unitOfWork.Articles.GetArticleByIdAsync(id);
            return article ?? null;
        }

        public async Task AddAsync(ArticleDto article)
        {
            await _unitOfWork.Articles.AddArticleAsync(article);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddArticleWithSourceAsync(ArticleDto articleDto, SourceDto sourceDto)
        {
            var sourceId = await _unitOfWork.Sources.AddSourceAsync(sourceDto);

            articleDto.SourceId = sourceId;
            await _unitOfWork.Articles.AddArticleAsync(articleDto);
            
            await _unitOfWork.SaveChangesAsync();
        }

    }
}