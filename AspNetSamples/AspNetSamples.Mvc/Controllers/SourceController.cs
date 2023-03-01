using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetSamples.Mvc.Controllers
{
    public class SourceController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSourceModel model)
        {
            ModelState.AddModelError("A", "12312312312");
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errors = ModelState.ErrorCount;
                foreach (var element in ModelState)
                {
                    if (element.Value.ValidationState == ModelValidationState.Invalid)
                    {
                        var errMsg = element.Key;
                    }
                }
                return View(model);
            }
        }
    }
}
