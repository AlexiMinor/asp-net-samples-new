using System.Text;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Business;
using AspNetSamples.Data;
using AspNetSamples.Data.Entities;
using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IConfiguration _configuration;

        public ArticleController(IArticleService articleService, 
            ISourceService sourceService, 
            IConfiguration configuration)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page=1)
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

                var articles = await _articleService
                    .GetArticlesWithSourceNoTrackingAsQueryable()
                    //.OrderBy(article => article.Id)    
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(article => new ArticlePreviewModel
                    {
                        Id = article.Id,
                        ShortDescription = article.ShortDescription,
                        Title = article.Title,
                        SourceName = article.Source.Name
                    })
                    .ToListAsync();


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
        public async Task<IActionResult> Details([FromRoute]ArticleSearchModel searchData)
        {
            var article = await _articleService.GetArticleByIdAsync(searchData.Id);

            return article != null
                ? View(article) 
                : NotFound();
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

        //[HttpGet]
        //public IActionResult Details2(string[] array)
        //{
        //    var sb = new StringBuilder("");
        //    foreach (var item in array)
        //    {
        //        sb.Append($"{item},");
        //    }

        //    return Content(sb.ToString());
        //}


        private Article Convert(CreateArticleModel model)
        {
            var entity = new Article
            {
                Title = model.Title,
                SourceId = model.SourceId,
                ShortDescription = model.ShortDescription,
                FullText = model.FullText
            };
            return entity;
        }
    }
}
