using System.Collections.Concurrent;
using System.Xml;
using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using HtmlAgilityPack;


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

        private async Task<string[]> GetContainsArticleUrlsBySourceAsync(int sourceId)
        {
            var articleUrls = await _unitOfWork.Articles.GetAsQueryable()
                .Where(article => article.SourceId.Equals(sourceId))
                .Select(article => article.ArticleSourceUrl)
                .ToArrayAsync();
            return articleUrls;
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

        public async Task AddArticlesAsync(IEnumerable<ArticleDto> articles)
        {
            var entities = articles.Select(a => _mapper.Map<Article>(a)).ToArray();

            await _unitOfWork.Articles.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();

        }

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

        public async Task<List<ArticleDto>> AggregateArticlesDataFromRssSourceAsync(SourceDto source, CancellationToken cancellationToken)
        {
            var articles = new ConcurrentBag<ArticleDto>();
            var urls = await GetContainsArticleUrlsBySourceAsync(source.Id);
            using (var reader = XmlReader.Create(source.RssFeedUrl))
            {
                var feed = SyndicationFeed.Load(reader);

                await Parallel.ForEachAsync(feed.Items
                        .Where(item => !urls.Contains(item.Id)).ToArray(), cancellationToken,
                    (item, token) => 
                    {
                            articles.Add(new ArticleDto()
                            {
                                ArticleSourceUrl = item.Id,
                                SourceId = source.Id,
                                SourceName = source.Name,
                                Title = item.Title.Text,
                                ShortDescription = item.Summary.Text
                            });
                            return ValueTask.CompletedTask;
                    });

               reader.Close();
            }

            return articles.ToList();
        }

        public async Task<List<ArticleDto>> GetFullContentArticlesAsync(List<ArticleDto> articlesDataFromRss)
        {
            var concBag = new ConcurrentBag<ArticleDto>();

            await Parallel.ForEachAsync(articlesDataFromRss, async (dto, token) =>
            {
                var content = await GetArticleContentAsync(dto.ArticleSourceUrl);
                dto.FullText = content;
                concBag.Add(dto);
            });
            return concBag.ToList();
        }

        private async Task<string> GetArticleContentAsync(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var textNode = doc.DocumentNode.SelectSingleNode("//div[@class = 'news-text']");

                //var nodesForDelete = textNode.SelectNodes("//div[@class = 'news-reference']");
                
                //if (nodesForDelete.Any())
                //{
                //    textNode.RemoveChildren(nodesForDelete);
                //}


                var content = textNode.InnerHtml;

                return content;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }
    }
}