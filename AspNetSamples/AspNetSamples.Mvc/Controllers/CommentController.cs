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
    }
}
