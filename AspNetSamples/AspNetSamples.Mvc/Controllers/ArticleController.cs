using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Mvc.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using ILogger = Serilog.ILogger;


namespace AspNetSamples.Mvc.Controllers
{
    //[Authorize(Policy = "18+Content")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ICommentService _commentService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService,
            ISourceService sourceService,
            IConfiguration configuration,
            ICommentService commentService,
            IMapper mapper)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _configuration = configuration;
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            //Log.Information("Hello there");
           try
            {
                var totalArticlesCount = await _articleService.GetTotalArticlesCountAsync();
                //Log.Debug("Count of articles was gotten successfully");
                if (int.TryParse(_configuration["Pagination:Articles:DefaultPageSize"], out var pageSize))
                {
                    var pageInfo = new PageInfo()
                    {
                        PageSize = pageSize,
                        PageNumber = page,
                        TotalItems = totalArticlesCount
                    };

                    var articleDtos = await _articleService
                        .GetArticlesByPageAsync(page, pageSize);

                    var articles = articleDtos
                        .Select(dto =>
                            _mapper.Map<ArticlePreviewModel>(dto))
                        .ToList();

                    return View(new ArticlesWithPaginationModel()
                    {
                        ArticlePreviews = articles,
                        PageInfo = pageInfo
                    });
                }

                else
                {
                    Log.Warning("Trying to get page with incorrect pageNumber");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var articleDto = await _articleService.GetArticleByIdWithSourceNameAsync(id);

            if (articleDto != null)
            {
                var comments = await _commentService.GetCommentsByArticleIdAsync(articleDto.Id);

                var model = new ArticleDetailsWithCreateCommentModel()
                {
                    ArticleDetails = new ArticleDetailsModel()
                    {
                        Id = articleDto.Id,
                        Title = articleDto.Title,
                        Rate = articleDto.Rate,
                        SourceName = articleDto.SourceName,
                        FullText = articleDto.FullText
                    },
                    Comments = comments.Select(dto => new CommentModel()
                    {
                        User = dto.User,
                        ArticleId = dto.ArticleId,
                        CommentText = dto.CommentText
                    }).ToList(),
                    CreateComment = new CommentModel()
                    {
                        ArticleId = articleDto.Id
                    }
                };

                return View(model);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateArticleModel()
            {
                AvailableSources = (await _sourceService.GetSourcesAsync())
                    .Select(source => new SelectListItem(source.Name, source.Id.ToString()))
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleModel model)
        {
            if (ModelState.IsValid)
            {
                await _articleService.AddAsync(Convert(model));
                return RedirectToAction("Index", "Article");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CreateArticleWithSource()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleWithSource(CreateArticleWithSourceModel model)
        {
            var source = new SourceDto()
            {
                Name = model.SourceName
            };

            var articleDto = new ArticleDto()
            {
                Title = model.Title,
                FullText = model.FullText,
                ShortDescription = model.ShortDescription,
            };
            await _articleService.AddAsync(articleDto);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesNames(string name="")
        {
            try
            {
                var names = await _articleService.GetArticlesNamesByPartNameAsync(name);
                return Ok(names);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Aggregator()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AggregateNews()
        {
            var sources = (await _sourceService.GetSourcesAsync())
                .Where(s=>!string.IsNullOrEmpty(s.RssFeedUrl))
                .ToArray();

            foreach (var sourceDto in sources)
            {
                var articlesDataFromRss = (await _articleService
                    .AggregateArticlesDataFromRssSourceAsync(sourceDto, CancellationToken.None));

                var fullContentArticles = await _articleService.GetFullContentArticlesAsync(articlesDataFromRss);

                await _articleService.AddArticlesAsync(fullContentArticles);
            }

            return Ok();
        }

        //todo move to mapper
        private ArticleDto Convert(CreateArticleModel model)
        {
            var dto = new ArticleDto()
            {
                Title = model.Title,
                SourceId = model.SourceId,
                ShortDescription = model.ShortDescription,
                FullText = model.FullText
            };
            return dto;
        }

        
    }
}
