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
        private readonly ITicketsService _service;
        private readonly IUsersService _usersService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ITicketsService service, IUsersService usersService, ILogger<TicketsController> logger)
        {
            _service = service;
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<List<TicketDto>> Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            if (user == null)
                return Forbid();

            if (user.Group == "User")
                return Ok(_service.GetTicketsByUser(user));

            return Ok(_service.GetTickets());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator,User")]
        public ActionResult<TicketDto> Get(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _usersService.GetUserFromToken(token);

            if (user == null)
                return Forbid();

            var foundTicket = _service.GetTicket(id);

            if (foundTicket == null) return NotFound();

            if (user.Group == "User")
            {
                if (foundTicket.RequesterId == user.Id)
                    return Ok(foundTicket);
                else
                    return Forbid();
            }

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
