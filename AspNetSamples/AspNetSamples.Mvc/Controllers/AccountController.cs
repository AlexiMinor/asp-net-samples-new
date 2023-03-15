using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetSamples.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private static List<string> _emails = new List<string>
        {
            "123@em.ail",
            "123@ema.il"
        };

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            return Ok(model);
        }

        [HttpGet]
        public IActionResult CheckIsUserEmailIsValidAndNotExists(string email)
        {
            if (_emails.Contains(email))
            {
                return Ok(false);
            }
            return Ok(true);
        }
    }
}
 