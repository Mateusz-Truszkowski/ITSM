using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                    TestUtil.TestData.CreateTestTicket1(),
                    TestUtil.TestData.CreateTestTicket2()
                );

                context.SaveChanges();
            }

            return context;
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ticket, TicketDto>().ForMember(dest => dest.Requester, opt => opt.Ignore());
                cfg.CreateMap<TicketDto, Ticket>();
                cfg.CreateMap<User, UserDto>();
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

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1())), JsonConvert.SerializeObject(result));
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
            var ticketDto = mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1());

            var result = service.CreateTicket(ticketDto);

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1())), JsonConvert.SerializeObject(result));
            var createdTicket = mapper.Map<TicketDto>(context.Tickets.FirstOrDefault(t =>  t.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1())), JsonConvert.SerializeObject(createdTicket));
        }

        [Fact]
        public void UpdateTicket_UpdatesTicket_WhenTicketExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);
            var ticketDto = mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1());
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
            var ticketDto = mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1());

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

        [Fact]
        public void GetTicketsByUser_ReturnsListOfTickets_WhenUserHaveTickets()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);

            var result = service.GetTicketsByUser(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1()));

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void GetTicketsByUser_ReturnsEmptyList_WhenUserDoesNotHaveTickets()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new TicketsService(context, mapper);

            var result = service.GetTicketsByUser(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser2()));

            Assert.Empty(result);
        }
    }
}
