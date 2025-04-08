using ITSM.Dto;
using ITSM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketsService _service;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(TicketsService service, ILogger<TicketsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<List<TicketDto>> Get()
        {
            return Ok(_service.GetTickets());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<TicketDto> Get(int id)
        {
            var foundTicket = _service.GetTicket(id);

            if (foundTicket == null) return NotFound();

            return Ok(foundTicket);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<TicketDto> Post([FromBody] TicketDto ticket)
        {
            var createdService = _service.CreateTicket(ticket);
            return CreatedAtAction(nameof(Get), new {id =  createdService.Id}, createdService);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult<TicketDto> Patch([FromBody] TicketDto ticket)
        {
            var updatedTicket = _service.UpdateTicket(ticket);

            if (updatedTicket == null) return NotFound();

            return Ok(updatedTicket);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult Delete(int id)
        {
            _service.DeleteTicket(id);
            return NoContent();
        }
    }
}
