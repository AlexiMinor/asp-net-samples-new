using System.Text;
using AspNetSamples.Data;
using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly NewsAggregatorContext _dbContext;

        public ArticleController(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.Articles.ToListAsync();
            if (Request.Query.ContainsKey("ad"))
            {
                var adSource = Request.Query["ad"];
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromQuery]ArticleSearchModel searchData)
        {
            var article = await _dbContext.Articles
                .FirstOrDefaultAsync(a => a.Id.Equals(searchData.Id));


            return article == null
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
