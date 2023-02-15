using System.Text;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Business;
using AspNetSamples.Data;
using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;


        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = 
                (await _articleService.GetArticlesWithSourceAsync())
                .Select(article => new ArticlePreviewModel
                {
                    Id = article.Id,
                    ShortDescription = article.ShortDescription,
                    Title = article.Title,
                    SourceName = article.Source.Name
                })
                .ToList(); 

            if (Request.Query.ContainsKey("ad"))
            {
                var adSource = Request.Query["ad"];
            }
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute]ArticleSearchModel searchData)
        {
            var article = await _articleService.GetArticleByIdAsync(searchData.Id);

            return article != null
                ? View(article) 
                : NotFound();
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
    }
}
