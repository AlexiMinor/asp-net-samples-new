using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetSamples.Mvc.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Register(RegisterModel model)
        {
            
        }

        [HttpGet]
        public IActionResult CheckIsUserEmailIsValidAndNotExists()
        {
            //go to db
            //check that user is exists
            //if no
            return Ok(true);
            //else
            return Ok(false);

        }
    }
}
