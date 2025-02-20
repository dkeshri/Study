using AuthService.Dtos;
using AuthService.Interfaces;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IAuthenticationService authenticationService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger,IAuthenticationService authenticationService)
        {
            _logger = logger;
            this.authenticationService = authenticationService;
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult<AuthenticatedUser> Login(UserCredientialsDto userCredientialsDto)
        {
            AuthenticatedUser? AuthenticatedUser = authenticationService.Login(userCredientialsDto);
            if (AuthenticatedUser == null)
            {
                _logger.LogInformation($"Invalid UserName: {userCredientialsDto.UserName} and Password: {userCredientialsDto.Password} ");
                return Unauthorized();
            }

            return Ok(AuthenticatedUser);
        }
    }
}
