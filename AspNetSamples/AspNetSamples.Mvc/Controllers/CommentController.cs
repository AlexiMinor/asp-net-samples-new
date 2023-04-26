using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.Mvc.Controllers
{
    public class CommentController : Controller
    {
        [HttpPost]
        public IActionResult Create(CommentModel model)
        {
            return RedirectToAction("Index","Article");
        }


        [HttpGet]
        public IActionResult GetFakeComments()
        {
            var list = new List<CommentModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new CommentModel()
                {
                    ArticleId = 1,
                    CommentText = $"Some text {i}",
                    User = $"User {i}"
                });
            }

            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetFakeCommentsPartial()
        {
            var list = new List<CommentModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new CommentModel()
                {
                    ArticleId = 1,
                    CommentText = $"Some text {i}",
                    User = $"User {i}"
                });
            }

            return View(list);
        }
    }
}
