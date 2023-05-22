using AspNetSamples.Abstractions.Services;
using AspNetSamples.WebAPI.Requests;
using AspNetSamples.WebAPI.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ITokenService _tokenService;

        public TokenController(IUserService userService, 
            IJwtService jwtService, 
            ITokenService tokenService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _tokenService = tokenService;
        }
        //123@em.ail
        //J799MtK87vD4P6B

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)//add some info about IP/machine name/etc
        {

            if (await _userService.IsUserExistsAsync(loginRequest.Email)
                && await _userService.IsPasswordCorrectAsync(loginRequest.Email, loginRequest.Password))
            {
                var user = await _userService.GetUserByEmailAsync(loginRequest.Email);

                if (user != null)
                {
                    var jwtTokenString = await _jwtService.GetTokenAsync(user);

                    var refreshToken = Guid.NewGuid();
                    await _tokenService.AddRefreshTokenAsync(user.Id, refreshToken);//can contain info about device


                    return Ok(new TokenResponse()
                    {
                        JwtToken = jwtTokenString,
                        RefreshToken = refreshToken.ToString("D"),
                        Email = user.Email
                    });
                }
                else
                {
                    return BadRequest("Invalid user");
                }
            }
            else
            {
                return BadRequest("Invalid user or credentials");
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {

           var tokens =  await _tokenService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);

           return Ok(new TokenResponse()
           {
               JwtToken = tokens.JwtToken,
               RefreshToken = tokens.RefreshToken,
           });
        }

    }
}
