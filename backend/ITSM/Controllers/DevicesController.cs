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
        private readonly ILogger<DevicesController> _logger;

        public DevicesController (DevicesService service, ILogger<DevicesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<List<DeviceDto>> Get()
        {
            return Ok(_service.GetDevices());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<DeviceDto> Get(int id)
        {
            var foundDevice = _service.GetDeviceById(id);

            if (foundDevice == null) return NotFound();

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
