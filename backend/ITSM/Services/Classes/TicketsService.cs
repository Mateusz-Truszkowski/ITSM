using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.Linq;
using System.ComponentModel;


namespace ITSM.Services
{
    public class TicketsService : ITicketsService
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

        public List<TicketDto> GetTicketsByUser(UserDto user)
        {
            var tickets = _context.Tickets.Where(t => t.RequesterId == user.Id);
            return tickets.Select(t => _mapper.Map<TicketDto>(t)).ToList();
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

        public byte[] AllTicketsReport()
        {
            var tickets = _context.Tickets.ToList();
            var dtoList = tickets.Select(t => _mapper.Map<TicketDto>(t)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tickets");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "CreationDate";
            worksheet.Cell(1, 5).Value = "SolutionDate";
            worksheet.Cell(1, 6).Value = "SolutionDescription";
            worksheet.Cell(1, 7).Value = "Priority";
            worksheet.Cell(1, 8).Value = "Type";
            worksheet.Cell(1, 9).Value = "Status";
          //  worksheet.Cell(1, 10).Value = "Service";
           // worksheet.Cell(1, 11).Value = "Requester";
           // worksheet.Cell(1, 12).Value = "Assignee";

            // Dodanie danych
            for (int i = 0; i < dtoList.Count; i++)
            {
                var ticket = dtoList[i];
                worksheet.Cell(i + 2, 1).Value = ticket.Id;
                worksheet.Cell(i + 2, 2).Value = ticket.Name;
                worksheet.Cell(i + 2, 3).Value = ticket.Description;
                worksheet.Cell(i + 2, 4).Value = ticket.CreationDate;
                worksheet.Cell(i + 2, 5).Value = ticket.SolutionDate;
                worksheet.Cell(i + 2, 6).Value = ticket.SolutionDescription;
                worksheet.Cell(i + 2, 7).Value = ticket.Priority;
                worksheet.Cell(i + 2, 8).Value = ticket.Type;
                worksheet.Cell(i + 2, 9).Value = ticket.Status;
                //worksheet.Cell(i + 2, 10).Value = ticket.Service.Name;
               // worksheet.Cell(i + 2, 11).Value = ticket.Requester.Name;
               // worksheet.Cell(i + 2, 12).Value = ticket.Assignee.Email;
            }

            
            using var stream = new System.IO.MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public byte[] TicketsReportForUser(UserDto user)
        {
            var tickets = _context.Tickets.Where(t => t.RequesterId == user.Id);
            var dtoList = tickets.Select(t => _mapper.Map<TicketDto>(t)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tickets");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "CreationDate";
            worksheet.Cell(1, 5).Value = "SolutionDate";
            worksheet.Cell(1, 6).Value = "SolutionDescription";
            worksheet.Cell(1, 7).Value = "Priority";
            worksheet.Cell(1, 8).Value = "Type";
            worksheet.Cell(1, 9).Value = "Status";
            worksheet.Cell(1, 10).Value = "Service";
            worksheet.Cell(1, 11).Value = "Requester";
            worksheet.Cell(1, 12).Value = "Assignee";

            for (int i = 0; i < dtoList.Count; i++)
            {
                var ticket = dtoList[i];
                worksheet.Cell(i + 2, 1).Value = ticket.Id;
                worksheet.Cell(i + 2, 2).Value = ticket.Name;
                worksheet.Cell(i + 2, 3).Value = ticket.Description;
                worksheet.Cell(i + 2, 4).Value = ticket.CreationDate;
                worksheet.Cell(i + 2, 5).Value = ticket.SolutionDate;
                worksheet.Cell(i + 2, 6).Value = ticket.SolutionDescription;
                worksheet.Cell(i + 2, 7).Value = ticket.Priority;
                worksheet.Cell(i + 2, 8).Value = ticket.Type;
                worksheet.Cell(i + 2, 9).Value = ticket.Status;
                worksheet.Cell(i + 2, 10).Value = ticket.Service.Name;
                worksheet.Cell(i + 2, 11).Value = ticket.Requester.Name;
                worksheet.Cell(i + 2, 12).Value = ticket.Assignee.Email;
            }

            
            using (var memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
