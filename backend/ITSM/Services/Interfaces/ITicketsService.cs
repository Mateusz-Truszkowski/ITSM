using ITSM.Dto;

namespace ITSM.Services
{
    public interface ITicketsService
    {
        public List<TicketDto> GetTickets();
        public TicketDto? GetTicket(int id);
        public List<TicketDto> GetTicketsByUser(UserDto user);
        public TicketDto CreateTicket(TicketDto ticketDto);
        public TicketDto? UpdateTicket(TicketDto ticketDto);
        public void DeleteTicket(int id);
    }
}
