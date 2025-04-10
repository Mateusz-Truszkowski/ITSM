using ITSM.Services;
using ITSM.Tests.TestUtil;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using ITSM.Dto;
using AutoMapper;
using ITSM.Entity;
using System.Text;
using ITSM.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace ITSM.Tests.Integration
{
    [Collection("IntegrationTests")]
    public class TicketsControllerIntegrationTests : IClassFixture<MyWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JwtTokenService _tokenService;
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        private TicketDto testTicket1;
        private TicketDto testTicket2;
        private TicketDto testTicket3;

        public TicketsControllerIntegrationTests(MyWebApplicationFactory factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _client = factory.CreateClient();
            _tokenService = new JwtTokenService(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());

            using (var scope = factory.Services.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ITSMContext>();

                ResetDb();
            }

            _context = factory.Services.GetRequiredService<ITSMContext>();
            _mapper = GetMapper();

            testTicket1 = _mapper.Map<TicketDto>(TestData.CreateTestTicket1());
            testTicket2 = _mapper.Map<TicketDto>(TestData.CreateTestTicket2());
            testTicket3 = _mapper.Map<TicketDto>(TestData.CreateTestTicket3());
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ticket, TicketDto>()
                    .ForMember(dest => dest.Requester, opt => opt.Ignore())
                    .ForMember(dest => dest.Assignee, opt => opt.Ignore());
                cfg.CreateMap<TicketDto, Ticket>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task Get_ReturnsListOfTickets_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/tickets");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<TicketDto> { testTicket1, testTicket2 } ).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Get_ReturnsListOfOwnedTickets_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/tickets");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<TicketDto> { testTicket1 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsTicket_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/tickets/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testTicket1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsOwnedTicket_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/tickets/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testTicket1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/tickets/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/tickets/2");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.GetAsync("/tickets/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreatesTicket_WhenValidTokenProvided()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/tickets", 
                new StringContent(
                    JsonConvert.SerializeObject(testTicket3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testTicket3).ToLower(), json.ToLower());
            var createdTicket = _mapper.Map<TicketDto>(_context.Tickets.FirstOrDefault(t => t.Id == 3));
            Assert.Equal(JsonConvert.SerializeObject(testTicket3), JsonConvert.SerializeObject(createdTicket));
        }

        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.PostAsync("/tickets",
                new StringContent(
                    JsonConvert.SerializeObject(testTicket3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_UpdatesTicket_WhenTicketExists()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);
            var requestTicket = testTicket3;
            requestTicket.Id = 2;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/tickets",
                new StringContent(
                    JsonConvert.SerializeObject(requestTicket),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(requestTicket).ToLower(), json.ToLower());
            var updatedTicket = _mapper.Map<TicketDto>(_context.Tickets.FirstOrDefault(t => t.Id == 2));
            Assert.Equal(JsonConvert.SerializeObject(requestTicket), JsonConvert.SerializeObject(updatedTicket));
        }

        [Fact]
        public async Task Patch_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/tickets",
                new StringContent(
                    JsonConvert.SerializeObject(testTicket3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsForbidden_WhenUserGroupIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/tickets",
                new StringContent(
                    JsonConvert.SerializeObject(testTicket1),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.PatchAsync("/tickets",
                new StringContent(
                    JsonConvert.SerializeObject(testTicket1),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletesTicket_WhenTicketExists()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/tickets/2");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var deletedTicket = _context.Tickets.FirstOrDefault(t => t.Id == 2);
            Assert.Null(deletedTicket);
        }

        [Fact]
        public async Task Delete_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.DeleteAsync("/tickets/2");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private void ResetDb()
        {
            _context.Database.EnsureDeleted();
            _context.ChangeTracker.Clear();

            _context.Users.AddRange(
                TestData.CreateTestUser1(),
                TestData.CreateTestUser2(),
                TestData.CreateTestUser3()
            );

            _context.Devices.AddRange(
                TestData.CreateTestDevice1(),
                TestData.CreateTestDevice2()
            );

            _context.Tickets.AddRange(
                TestData.CreateTestTicket1(),
                TestData.CreateTestTicket2()
            );
            _context.SaveChanges();
        }
    }
}
