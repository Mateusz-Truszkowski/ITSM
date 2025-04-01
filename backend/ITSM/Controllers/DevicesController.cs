using ITSM.Dto;
using ITSM.Services;
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
        public ActionResult<List<DeviceDto>> Get()
        {
            return Ok(_service.GetDevices());
        }

        [HttpGet("{id}")]
        public ActionResult<DeviceDto> Get(int id)
        {
            return Ok(_service.GetDeviceById(id));
        }

        [HttpPost]
        public ActionResult<DeviceDto> Post([FromBody] DeviceDto deviceDto)
        {
            var device = _service.CreateDevice(deviceDto);
            return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
        }

        [HttpPatch]
        public ActionResult<DeviceDto> Patch([FromBody] DeviceDto deviceDto)
        {
            return Ok(_service.UpdateDevice(deviceDto));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _service.DeleteDevice(id);
            return NoContent();
        }
    }
}
