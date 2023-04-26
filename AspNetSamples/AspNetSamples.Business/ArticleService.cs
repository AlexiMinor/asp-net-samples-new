using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Business
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            try
            {
                var articles = (await _unitOfWork
                        .Articles
                        .GetArticlesByPageAsync(page, pageSize))
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToList();

                //var x = 0;
                //var z = 15 / x;

                return articles;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

        public async Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(string partName)
        {
            var articles = await _unitOfWork.Articles
                .GetAsQueryable()
                .AsNoTracking()
                .Where(article => article.Title.Contains(partName))
                .Select(article => _mapper.Map<AutoCompleteDataDto>(article))
                .ToListAsync();

            return articles;
                
        }


        //public async Task<int> AddArticleWithNewSourceAsync()
        //{
        //    _unitOfWork.Articles
        //        .FindBy(article => !string.IsNullOrEmpty(article.FullText),
        //            article => article.Source,
        //            article => article.Comments)
        //        .Select(article => new ArticleDto(){})
        //        .ToListAsync();
        //    return await _unitOfWork.SaveChangesAsync();
        //}

        //public async Task<Task<ArticleDto?>> GetArticleByIdAsync(int id)
        //{
        //    var article = _articleRepository.GetArticleByIdAsync(id);

        //    return article;
        //}

        public async Task<ArticleDto?> GetArticleByIdWithSourceNameAsync(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);

            return
                article != null
                    ? _mapper.Map<ArticleDto>(article)
                    : null;
        }

        public async Task AddAsync(ArticleDto dto)
        {
            await _unitOfWork.Articles.AddAsync(_mapper.Map<Article>(dto));
            await _unitOfWork.SaveChangesAsync();
        }

        //public async Task<int> AddArticlesAsync()
        //{
        //    var articlesForAdd = new List<Article>();

        //    //_unitOfWork.Articles.AddArticlesOptimizedWay()
        //}

        public async Task AddArticleWithSourceAsync(
                ArticleDto articleDto,
                SourceDto sourceDto)
        {
            var entry = await _unitOfWork.Sources.AddAsync(
                _mapper.Map<Source>(sourceDto));

            articleDto.SourceId = entry.Entity.Id;
            await _unitOfWork.Articles.AddAsync(_mapper.Map<Article>(articleDto));

            await _unitOfWork.SaveChangesAsync();
        }




    }
}