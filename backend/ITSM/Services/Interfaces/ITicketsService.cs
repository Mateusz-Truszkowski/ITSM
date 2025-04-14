using ITSM.Dto;
using Microsoft.AspNetCore.Mvc;

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
        public byte[] AllTicketsReport();
        public byte[] TicketsReportForUser(UserDto user);
    }
}
