using System.Security.Claims;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetSamples.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, 
            IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.RegisterAsync(model.Email, model.Password);
                if (user != null)
                {
                    await AuthenticateAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Smth goes wrong");
            }
            return View(model);
        }  
        [HttpGet]
        public async Task<IActionResult> Login([FromQuery]string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                var lm = new LoginModel()
                {
                    ReturnUrl = returnUrl
                };
                return View(lm);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (await _userService.IsUserExistsAsync(model.Email)
                && await _userService.IsPasswordCorrectAsync(model.Email, model.Password))
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);
                await AuthenticateAsync(user);

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
                
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithJS([FromBody]LoginModel model)
        {
            if (await _userService.IsUserExistsAsync(model.Email)
                && await _userService.IsPasswordCorrectAsync(model.Email, model.Password))
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);
                await AuthenticateAsync(user);

                return Ok();
            }

            return BadRequest(model);
        }


        [HttpGet]
        public async Task<IActionResult> CheckIsUserEmailIsValidAndNotExists(string email)
        {
           return Ok(!await _userService.IsUserExistsAsync(email));
        }

        [HttpGet]
        public async Task<IActionResult> IsUserAuthenticated()
        {
            if (HttpContext.User?.Identity != null)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        private async Task AuthenticateAsync(UserDto dto)
        {
            try
            {
                const string authType = "Application Cookie";
                var claims = await _userService.GetUserClamsAsync(dto);
                var identity = new ClaimsIdentity(claims,
                    authType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
 