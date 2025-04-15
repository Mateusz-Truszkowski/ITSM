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
        private readonly IDevicesService _service;
        private readonly IUsersService _usersService;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController (IDevicesService service, IUsersService usersService, ILogger<DevicesController> logger)
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
        [HttpGet("report")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult MakeDevicesReport()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            byte[] file_data;
            string file_name;


            if (user == null)
                return Forbid();

            if (user.Group == "User")
            {
                file_data = _service.UserDevicesReport(user);
                file_name = $"Report_My_Devices_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";
            }
            else
            {
                file_data = _service.AllDevicesReport();
                file_name = $"Report_All_Devices_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";
            }

            if (file_data == null || file_data.Length == 0)
                return StatusCode(500, "Błąd podczas generowania raportu (plik jest pusty).");

            return File(file_data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name);
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
