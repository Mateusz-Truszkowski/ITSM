using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public class TicketsService
    {
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        public TicketsService(ITSMContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<TicketDto> GetTickets()
        {
            return _context.Tickets.Select(t => _mapper.Map<TicketDto>(t)).ToList();
        }

        public TicketDto? GetTicket(int id)
        {
            var ticket = _context.Tickets.Where(t => t.Id == id).FirstOrDefault();
            if (ticket == null)
                return null;

            return _mapper.Map<TicketDto>(ticket);
        }

        public TicketDto CreateTicket(TicketDto ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return _mapper.Map<TicketDto>(ticket);
        }

        public TicketDto? UpdateTicket(TicketDto ticketDto)
        {
            var ticket = _context.Tickets.Where(t => t.Id == ticketDto.Id).FirstOrDefault();
            if (ticket == null)
                return null;

            if (!string.IsNullOrEmpty(ticketDto.Name))
                ticket.Name = ticketDto.Name;

            if (!string.IsNullOrEmpty(ticketDto.Description))
                ticket.Description = ticketDto.Description;

            if (ticketDto.CreationDate != DateTime.MinValue)
                ticket.CreationDate = ticketDto.CreationDate;

            if (ticketDto.SolutionDate != DateTime.MinValue)
                ticket.SolutionDate = ticketDto.SolutionDate;

            if (!string.IsNullOrEmpty(ticketDto.SolutionDescription))
                ticket.SolutionDescription = ticketDto.SolutionDescription;

            if (ticketDto.Priority != 0)
                ticket.Priority = ticketDto.Priority;
            
            if (!string.IsNullOrEmpty(ticketDto.Type))
                ticket.Type = ticketDto.Type;

            if (!string.IsNullOrEmpty(ticketDto.Status))
                ticket.Status = ticketDto.Status;

            if (ticketDto.ServiceId != 0)
                ticket.ServiceId = ticketDto.ServiceId;

            if (ticketDto.RequesterId != 0)
                ticket.RequesterId = ticketDto.RequesterId;

            if (ticketDto.AssigneeId != 0)
                ticket.AssigneeId = ticketDto.AssigneeId;

            _context.SaveChanges();

            return _mapper.Map<TicketDto>(ticket);
        }

        public void DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Where(t => t.Id == id).FirstOrDefault();
            if (ticket == null)
                return;

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
        }
    }
}
