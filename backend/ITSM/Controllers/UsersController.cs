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
        public ActionResult<List<UserDto>> Get()
        {
            return Ok(_usersService.GetUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> Get(int id)
        {
            var user = _usersService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<UserDto> Post(CreateUserDto user)
        {
            var createdUser = _usersService.CreateUser(user);
            return CreatedAtAction(nameof(Get), new {id = createdUser.Id}, createdUser);
        }

        [HttpPatch]
        public ActionResult<UserDto> Patch(UserDto user)
        {
            return Ok(_usersService.UpdateUser(user));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _usersService.DeleteUser(id);
            return NoContent();
        }
    }
}
