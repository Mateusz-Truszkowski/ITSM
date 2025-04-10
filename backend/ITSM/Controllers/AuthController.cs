using ITSM.Dto;
using ITSM.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace ITSM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly IUsersService _usersService;

        public AuthController(JwtTokenService jwtTokenService, IUsersService usersService)
        {
            _jwtTokenService = jwtTokenService;
            _usersService = usersService;
        }

        [HttpPost("/auth/login")]
        public ActionResult<Response> Login([FromBody] Credentials credentials)
        {
            if (_usersService.Authenticate(credentials.Login, credentials.Password))
            {
                var user = _usersService.GetUserByLogin(credentials.Login);
                if (user != null)
                    return Ok(new Response { User = user, Token = _jwtTokenService.GenerateToken(user.Login, user.Group) });
                else
                    return StatusCode(500, "Existing user doesn't exists");
            }
            else
                return Unauthorized();
        }

        [HttpPost("/auth/reset")]
        public ActionResult Reset([FromBody] string Login)
        {
            var foundUser = _usersService.GetUserByLogin(Login);

            if (foundUser == null)
                return NotFound();

            return Ok("Mail został wysłany.");
            // Jeżeli znaleziono
            /*#try
             {
                 // Składanie wiadomości
                 var mail = new MailMessage();
                 mail.From = new MailAddress("twojemail@example.com");
                 mail.To.Add(foundUser.Email);
                 mail.Subject = "Reset hasa ITSM";
                 mail.Body = "Kliknij w link, aby zresetować hasło do systemu ITSM: https://example.com/reset?user=" + foundUser.Id;
                 mail.IsBodyHtml = false; // lub true, jeśli chcesz HTML

                 // Konfiguracja SMTP
                 var smtpClient = new SmtpClient("smtp.example.com")
                 {
                     Port = 587,
                     Credentials = new NetworkCredential("twojemail@example.com", "twojehaslo"),
                     EnableSsl = true,
                 };

                 smtpClient.Send(mail);

                 return Ok("Mail został wysłany.");
             }
             catch (Exception ex)
             {
                 // Logowanie błędu (w produkcji loguj to gdzieś!)
                 return StatusCode(500, "Wystąpił błąd podczas wysyłania maila: " + ex.Message);
             }*/
        }
    }

    public class Credentials
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }

    public class Response
    {
        public required UserDto User { get; set; }
        public required string Token { get; set; }
    }
}
