using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using Microsoft.AspNetCore.Authorization;

using System.Linq;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] LoginInputModel login)
        {
            var user = _accountService.AuthenticateUser(login);
            if (user == null) { return Unauthorized(); }
            return Ok(_tokenService.GenerateJwtToken(user));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterInputModel registeruser)
        {
            var user = _accountService.CreateUser(registeruser);
            if (user == null) { return Unauthorized(); }
            return Ok(_tokenService.GenerateJwtToken(user));
        }

        [HttpGet]
        [Route("signout")]
        public IActionResult Signout()
        {
            var a = User.Claims.FirstOrDefault(c => c.Type == "tokenId");
            if (a == null) { return StatusCode(400);} //kikja kannski á enn ætti ekki að gerast
            int tokenId = int.Parse(a.Value);
            _accountService.Logout(tokenId);
            return NoContent();
        }
    }
}