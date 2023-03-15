using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetSamples.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ICommentService _commentService;
        private readonly IConfiguration _configuration;

        public ArticleController(IArticleService articleService,
            ISourceService sourceService,
            IConfiguration configuration, ICommentService commentService)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _configuration = configuration;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var totalArticlesCount = await _articleService.GetTotalArticlesCountAsync();

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
                    .Select(article => new ArticlePreviewModel
                    {
                        Id = article.Id,
                        ShortDescription = article.ShortDescription,
                        Title = article.Title,
                        SourceName = article.SourceName
                    })
                .ToList();


                //if (Request.Query.ContainsKey("ad"))
                //{
                //    var adSource = Request.Query["ad"];
                //}
                return View(new ArticlesWithPaginationModel()
                {
                    ArticlePreviews = articles,
                    PageInfo = pageInfo
                });
            }

            else
            {
                return StatusCode(500, new { Message = "Can't read configuration data" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] ArticleSearchModel searchData)
        {
            var articleDto = await _articleService.GetArticleByIdWithSourceNameAsync(searchData.Id);

            if (articleDto!= null)
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
