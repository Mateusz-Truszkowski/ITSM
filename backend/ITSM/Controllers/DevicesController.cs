using ITSM.Dto;
using ITSM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesService _service;
        private readonly UsersService _usersService;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController (DevicesService service, UsersService usersService, ILogger<DevicesController> logger)
        {
            _service = service;
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<List<DeviceDto>> Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            if (user == null)
                return Forbid();

            if (user.Group == "User")
                return Ok(_service.GetDevicesByUser(user));

            return Ok(_service.GetDevices());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<DeviceDto> Get(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            if (user == null)
                return Forbid();

            var foundDevice = _service.GetDeviceById(id);

            if (foundDevice == null) return NotFound();

            if (user.Group == "User")
            {
                if (foundDevice.UserId == user.Id)
                    return Ok(foundDevice);
                else
                    return Forbid();
            }

            return Ok(foundDevice);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult<DeviceDto> Post([FromBody] DeviceDto deviceDto)
        {
            var device = _service.CreateDevice(deviceDto);
            return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult<DeviceDto> Patch([FromBody] DeviceDto deviceDto)
        {
            var updatedDevice = _service.UpdateDevice(deviceDto);

            if (updatedDevice == null) return NotFound();

            return Ok(updatedDevice);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult Delete(int id)
        {
            _service.DeleteDevice(id);
            return NoContent();
        }
    }
}
