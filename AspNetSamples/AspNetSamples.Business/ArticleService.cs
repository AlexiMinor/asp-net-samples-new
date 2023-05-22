using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Xml;
using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using AspNetSamples.Business.RateModels;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using AspNetSamples.Data.Migrations;
using Newtonsoft.Json;


namespace AspNetSamples.Business
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ArticleService(IUnitOfWork unitOfWork, 
            IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
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

        public async Task RateArticleAsync(int id, double? rate)
        {
            await _unitOfWork.Articles.PatchAsync(id, new List<PatchDto>()
            {
                new PatchDto()
                {
                    PropertyName = nameof(ArticleDto.Rate),
                    PropertyValue = rate
                }
            });

            await _unitOfWork.SaveChangesAsync();

        }

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

        public async Task<double?> GetArticleRateAsync(int articleId)
        {
            var articleText = (await _unitOfWork.Articles.GetByIdAsync(articleId))?.FullText;


            if (string.IsNullOrEmpty(articleText))
            {
                throw new ArgumentException("Article or article text doesn't exist",
                    nameof(articleId));
            }
            else
            {
                Dictionary<string, int>? dictionary;
                using (var jsonReader = new StreamReader(@"C:\Users\AlexiMinor\Desktop\asp-samples\asp-net-samples-new\AspNetSamples\AspNetSamples.Mvc\AFINN-ru.json"))
                {
                    var jsonDict = await jsonReader.ReadToEndAsync();
                    dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonDict);
                }
                
                articleText = PrepareText(articleText);

                using (var httpClient = new HttpClient())
                {
                    httpClient
                        .DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var request = new HttpRequestMessage(HttpMethod.Post,
                        "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=15031bb039d704a3af5d07194f427aa3bf297058")
                    {
                        Content = new StringContent("[{\"text\":\"" + articleText + "\"}]",
                            Encoding.UTF8, "application/json")
                    };

                    request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var lemmas = JsonConvert.DeserializeObject<Root[]>(responseString)
                            .SelectMany(root => root.Annotations.Lemma).Select(lemma => lemma.Value).ToArray();

                        if (lemmas.Any())
                        {
                            var totalRate = lemmas
                                .Where(lemma => dictionary.ContainsKey(lemma))
                                .Aggregate<string, double>(0, (current, lemma) 
                                    => current + dictionary[lemma]);

                            totalRate = totalRate / lemmas.Count();
                            return totalRate;
                        }
                    }
                    
                }
                return null;
            }
        }

        private string? PrepareText(string articleText)
        {
            articleText = articleText.Trim();

            articleText = Regex.Replace(articleText, "<.*?>", string.Empty);
            return articleText;
        }

        public async Task<List<ArticleDto>> GetUnratedArticlesAsync()
        {
            var unratedArticles = await _unitOfWork.Articles
                .GetAsQueryable()
                .AsNoTracking()
                .Where(article => article.Rate == null)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToListAsync();

            return unratedArticles;
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