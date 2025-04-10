using AutoMapper;
using ITSM.Controllers;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace ITSM.Tests.Controllers
{
    public class TicketsControllerTests : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly TicketsController _controller;
        private readonly Mock<ITicketsService> _ticketsServiceMock;
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly Mock<ILogger<TicketsController>> _loggerMock;

        private UserDto testUser1;
        private UserDto testUser2;
        private UserDto testUser3;

        private TicketDto testTicket1;
        private TicketDto testTicket2;

        public TicketsControllerTests() 
        {
            _mapper = GetMapper();
            _ticketsServiceMock = new Mock<ITicketsService>();
            _usersServiceMock = new Mock<IUsersService>();
            _loggerMock = new Mock<ILogger<TicketsController>>();
            _controller = new TicketsController(_ticketsServiceMock.Object, _usersServiceMock.Object, _loggerMock.Object);

            testUser1 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1());
            testUser2 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser2());
            testUser3 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser3());

            testTicket1 = _mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket1());
            testTicket2 = _mapper.Map<TicketDto>(TestUtil.TestData.CreateTestTicket2());

            Setup();
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
        public void Get_ReturnsOnlyOwnTickets_WhenUserIsInUserGroup()
        {
            _ticketsServiceMock.Setup(s => s.GetTicketsByUser(testUser1)).Returns(new List<TicketDto> { testTicket1 });
            _usersServiceMock.Setup(x => x.GetUserFromToken(It.IsAny<string>()))
                .Returns(testUser1);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tickets = Assert.IsAssignableFrom<List<TicketDto>>(okResult.Value);
            Assert.Single(tickets);
            Assert.Equal(JsonConvert.SerializeObject(new List<TicketDto> { testTicket1 }), JsonConvert.SerializeObject(tickets));
        }

        [Fact]
        public void Get_ReturnsAllTickets_WhenUserIsInAdminGroup()
        {
            _ticketsServiceMock.Setup(s => s.GetTickets()).Returns(new List<TicketDto> { testTicket1, testTicket2 });
            _usersServiceMock.Setup(x => x.GetUserFromToken(It.IsAny<string>()))
                .Returns(testUser3);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tickets = Assert.IsAssignableFrom<List<TicketDto>>(okResult.Value);
            Assert.Equal(2, tickets.Count);
            Assert.Equal(JsonConvert.SerializeObject(new List<TicketDto> { testTicket1, testTicket2 }), JsonConvert.SerializeObject(tickets));
        }

        [Fact]
        public void Get_ReturnsForbidden_WhenUserIsNull()
        {
            var result = _controller.Get();

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsTicket_WhenUserIsInAdminGroup()
        {
            Setup();
            _usersServiceMock.Setup(x => x.GetUserFromToken(It.IsAny<string>()))
                .Returns(testUser3);

            _ticketsServiceMock.Setup(s => s.GetTicket(1)).Returns(testTicket1);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ticket = Assert.IsAssignableFrom<TicketDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testTicket1), JsonConvert.SerializeObject(ticket));
        }

        [Fact]
        public void GetById_ReturnsTicket_WhenUserIsTicketOwner()
        {
            Setup();
            _usersServiceMock.Setup(x => x.GetUserFromToken(It.IsAny<string>()))
                .Returns(testUser1);

            _ticketsServiceMock.Setup(s => s.GetTicket(1)).Returns(testTicket1);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ticket = Assert.IsAssignableFrom<TicketDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testTicket1), JsonConvert.SerializeObject(ticket));
        }

        [Fact]
        public void GetById_ReturnsForbidden_WhenUserIdNotTicketOwner()
        {
            Setup();
            _usersServiceMock.Setup(x => x.GetUserFromToken(It.IsAny<string>()))
                .Returns(testUser2);

            _ticketsServiceMock.Setup(s => s.GetTicket(1)).Returns(testTicket1);

            var result = _controller.Get(1);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsForbidden_WhenUserIsNull()
        {
            Setup();

            _ticketsServiceMock.Setup(s => s.GetTicket(1)).Returns(testTicket1);

            var result = _controller.Get(1);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedTicket()
        {
            Setup();

            _ticketsServiceMock.Setup(s => s.CreateTicket(testTicket1)).Returns(testTicket1);

            var result = _controller.Post(testTicket1);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var ticket = Assert.IsAssignableFrom<TicketDto>(createdResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testTicket1), JsonConvert.SerializeObject(ticket));
        }

        [Fact]
        public void Patch_ReturnsUpdatedTicket_WhenTicketExists()
        {
            Setup();

            _ticketsServiceMock.Setup(s => s.UpdateTicket(testTicket1)).Returns(testTicket1);

            var result = _controller.Patch(testTicket1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ticket = Assert.IsAssignableFrom<TicketDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testTicket1), JsonConvert.SerializeObject(ticket));
        }

        [Fact]
        public void Patch_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            Setup();

            var result = _controller.Patch(testTicket1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
            Setup();

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        private void Setup()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Headers["Authorization"] = "Bearer fake-token";
        }

        public void Dispose()
        {
            _usersServiceMock.Reset();
        }
    }
}
