﻿using ITSM.Dto;
using ITSM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ILogger<ServicesController> _logger;
        private readonly IServicesService _service;

        public ServicesController (ILogger<ServicesController> logger, IServicesService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<List<ServiceDto>> Get()
        {
            return Ok(_service.GetServices());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<ServiceDto> Get(int id)
        {
            var foundService = _service.GetService(id);

            if (foundService == null) return NotFound();

            return Ok(foundService);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ServiceDto> Post([FromBody] ServiceDto service)
        {
            var createdService = _service.CreateService(service);
            return CreatedAtAction(nameof(Get), new { id = createdService.Id }, createdService);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public ActionResult<ServiceDto> Patch([FromBody] ServiceDto service)
        {
            var updatedService = _service.UpdateService(service);

            if (updatedService == null) return NotFound();

            return Ok(updatedService);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _service.DeleteService(id);
            return NoContent();
        }
    }
}
