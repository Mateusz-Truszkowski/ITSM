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
            return _service.GetTickets();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<TicketDto> Get(int id)
        {
            return _service.GetTicket(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<TicketDto> Post([FromBody] TicketDto ticket)
        {
            return _service.CreateTicket(ticket);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,Operator")]
        public ActionResult<TicketDto> Patch([FromBody] TicketDto ticket)
        {
            return _service.UpdateTicket(ticket);
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
