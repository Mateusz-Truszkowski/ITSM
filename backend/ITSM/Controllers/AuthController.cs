using ITSM.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly UsersService _usersService;

        public AuthController(JwtTokenService jwtTokenService, UsersService usersService)
        {
            _jwtTokenService = jwtTokenService;
            _usersService = usersService;
        }

        [HttpPost("/auth/login")]
        public ActionResult<string> Login([FromBody] Credentials credentials)
        {
            if (_usersService.Authenticate(credentials.Login, credentials.Password))
            {
                var user = _usersService.GetUserByLogin(credentials.Login);
                if (user != null)
                    return Ok(_jwtTokenService.GenerateToken(user.Login, user.Group));
                else
                    return StatusCode(500, "Existing user doesn't exists");
            }
            else
                return Unauthorized();
        }
    }

    public class Credentials
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
