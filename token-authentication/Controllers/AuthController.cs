using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using token_authentication.Models;

namespace token_authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var loginUser = FakeDatabase.Login(loginModel); // Login control with user on Fake Database
            if (loginUser is null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.UserName),
                new Claim(ClaimTypes.Role, "Manager")
            };

            var accessToken = _tokenService.GenereteAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            loginUser.RefreshToken = refreshToken;
            loginUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            FakeDatabase.SaveUser(loginUser); // saving RefreshToken & RefreshTokenExpiryTime

            return Ok(new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost, Route("Refresh")]
        public IActionResult Refresh(AuthenticatedResponse response)
        {
            if (response is null)
                return BadRequest();

            string accessToken = response.Token;
            string refreshToken = response.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = FakeDatabase.Login(username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _tokenService.GenereteAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            FakeDatabase.SaveUser(user);

            return Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize, Route("Logout")]
        public IActionResult Logout()
        {
            var username = User.Identity.Name;
            var user = FakeDatabase.Login(username);
            if (user == null)
            {
                return BadRequest();
            }

            user.RefreshToken = null;
            FakeDatabase.SaveUser(user);

            return NoContent();
        }

        [HttpGet, Authorize, Route("Test")]
        public IActionResult Test()
        {
            return Ok("You are logged in.");
        }

    }
}
