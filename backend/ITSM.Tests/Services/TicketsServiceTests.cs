using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests.Services
{
    public class TicketsServiceTests
    {
        private ITSMContext GetDbContext(bool empty = false)
        {
            var options = new DbContextOptionsBuilder<ITSMContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ITSMContext(options);

            if (!empty)
            {
                context.Tickets.AddRange(
                    TestUtil.TestData.CreateTestTicket()
                );

                context.SaveChanges();
            }

            return context;
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ticket, TicketDto>();
                cfg.CreateMap<TicketDto, Ticket>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public void GetTickets_ReturnsListOfTickets()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);

            var result = service.GetTickets();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetTicket_ReturnsTicket_WhenTicketExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);

            var result = service.GetTicket(1);

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket())), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetTicket_ReturnsNull_WhenTicketDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);

            var result = service.GetTicket(1);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTicket_CreatesTicket_WhenValidTicketDtoIsProvided()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);
            var ticketDto = mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket());

            var result = service.CreateTicket(ticketDto);

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket())), JsonConvert.SerializeObject(result));
            var createdTicket = mapper.Map<TicketDto>(context.Tickets.FirstOrDefault(t =>  t.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket())), JsonConvert.SerializeObject(createdTicket));
        }

        [Fact]
        public void UpdateTicket_UpdatesTicket_WhenTicketExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);
            var ticketDto = mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket());
            ticketDto.Description = "updated";

            var result = service.UpdateTicket(ticketDto);

            Assert.Equal(JsonConvert.SerializeObject(ticketDto), JsonConvert.SerializeObject(result));
            var updatedTicket = mapper.Map<TicketDto>(context.Tickets.FirstOrDefault(t => t.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(ticketDto), JsonConvert.SerializeObject(updatedTicket));
        }

        [Fact]
        public void UpdateTicket_ReturnsNull_WhenTicketDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);
            var ticketDto = mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket());

            var result = service.UpdateTicket(ticketDto);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteTicket_DeletesTicket_WhenTicketExists()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);

            service.DeleteTicket(1);

            var deletedTicket = context.Tickets.FirstOrDefault(t => t.Id == 1);
            Assert.Null(deletedTicket);
        }
    }
}
