using ITSM.Dto;
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
        private UsersService _usersService;

        public UsersController(ILogger<UsersController> logger, UsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult<List<UserDto>> Get()
        {
            return Ok(_usersService.GetUsers());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult<UserDto> Get(int id)
        {
            var user = _usersService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> Post(CreateUserDto user)
        {
            var createdUser = _usersService.CreateUser(user);
            return CreatedAtAction(nameof(Get), new {id = createdUser.Id}, createdUser);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> Patch(UserDto user)
        {
            var updatedUser = _usersService.UpdateUser(user);

            if (updatedUser == null) return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _usersService.DeleteUser(id);
            return NoContent();
        }
    }
}
