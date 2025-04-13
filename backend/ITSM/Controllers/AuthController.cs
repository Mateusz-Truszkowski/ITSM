using ITSM.Dto;
using ITSM.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using ITSM.Entity;
using Microsoft.AspNetCore.Authorization;

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
        [HttpPost("/auth/setnewpass")]
        public ActionResult<UserDto> Post([FromBody] NewPassRequest request)
        {
            var User = _usersService.GetUserFromToken(request.Token);

            if (request.NewPassword == null || User == null)
            {
                return Forbid();
            }

            _usersService.UpdateUserPassword(User.Id, request.NewPassword);

            return Ok(User.Login);
        }

        [HttpPost("/auth/reset")]
        public ActionResult Reset([FromBody] string Login)
        {
            var foundUser = _usersService.GetUserByLogin(Login);

            if (foundUser == null)
                return NotFound();

           
            //return Ok("Mail został wysłany.");
            // Jeżeli znaleziono
            try
             {
                // Składanie wiadomości

                if (!(foundUser.Email.Contains("@"))){
                    return NotFound();
                }
                string Token = _jwtTokenService.GenerateToken(foundUser.Login, foundUser.Group);
                string resetLink = $"http://localhost:5173/passwordResetFill?token={Token}";
                var mail = new MailMessage();

                mail.From = new MailAddress("itsmsystempostman@gmail.com", "ITSM System");
                mail.To.Add(new MailAddress(foundUser.Email, foundUser.Name));
                mail.Subject = "Reset hasa ITSM dla użytkownika " + foundUser.Login;
                mail.Body = "Kliknij w link, aby zresetować hasło do systemu ITSM: " + resetLink;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = false; 

                // Konfiguracja SMTP
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("itsmsystempostman@gmail.com", "qfmx zgoo ebfx hnax"),
                    EnableSsl = true,
                };

                smtpClient.Send(mail);

                return Ok("Mail został wysłany do uzytkownika:"+ foundUser.Login);
             }
             catch (Exception ex)
             {
                 // Logowanie błędu (w produkcji loguj to gdzieś!)
                 return BadRequest("Wystąpił błąd podczas wysyłania maila: " + ex.Message);
             }
        }
    }

    public class Credentials
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
    public class NewPassRequest
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
    public class Response
    {
        public required UserDto User { get; set; }
        public required string Token { get; set; }
    }
}
