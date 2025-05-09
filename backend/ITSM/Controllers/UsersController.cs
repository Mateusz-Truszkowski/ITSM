using DocumentFormat.OpenXml.Wordprocessing;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        public UsersController(ILogger<UsersController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<List<UserDto>> Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            if (user == null)
                return Forbid();

            if (user.Group == "User")
            {
                var foundUser = _usersService.GetUserById(user.Id);
                if (foundUser == null)
                    return Ok(new List<UserDto>());
                return Ok(new List<UserDto> { foundUser });
            }

            return Ok(_usersService.GetUsers());
        }
        [HttpGet("report")]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult MakeUsersReport()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);
            if (user == null)
                return Forbid();

            var file_data = _usersService.AllUsersReport();
            var file_name = $"Report_Users_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

            return File(file_data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name); ;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<UserDto> Get(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            if (user == null)
                return Forbid();

            var foundUser = _usersService.GetUserById(id);
            if (foundUser == null) return NotFound();

            if (user.Group == "User")
            {
                if (foundUser.Id == user.Id)
                    return Ok(foundUser);
                else
                    return Forbid();
            }

            return Ok(foundUser);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> Post([FromBody] CreateUserDto user)
        {
            var createdUser = _usersService.CreateUser(user);
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> Patch(UserDto user)
        {
            var updatedUser = _usersService.UpdateUser(user);

            if (updatedUser == null) return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _usersService.DeleteUser(id);
            return NoContent();
        }
    }
}
