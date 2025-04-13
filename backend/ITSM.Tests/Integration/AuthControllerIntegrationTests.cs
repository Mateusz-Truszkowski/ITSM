using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using ITSM.Tests.TestUtil;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Text;

namespace ITSM.Tests.Integration
{
    [Collection("IntegrationTests")]
    public class AuthControllerIntegrationTests : IClassFixture<MyWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JwtTokenService _tokenService;
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        private UserDto testUser1;
        private UserDto testUser2;
        private UserDto testUser3;
        private CreateUserDto testUser4;

        public AuthControllerIntegrationTests(MyWebApplicationFactory factory)
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

            testUser1 = _mapper.Map<UserDto>(TestData.CreateTestUser1());
            testUser2 = _mapper.Map<UserDto>(TestData.CreateTestUser2());
            testUser3 = _mapper.Map<UserDto>(TestData.CreateTestUser3());
            testUser4 = _mapper.Map<CreateUserDto>(TestData.CreateTestUser4());
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<CreateUserDto, User>();
                cfg.CreateMap<User, CreateUserDto>();
                cfg.CreateMap<CreateUserDto, UserDto>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task Post_ReturnsOk_WhenMailSent()
        {
            var user = TestData.CreateTestUser3();

            var response = await _client.PostAsync("/auth/reset",
                new StringContent(
                    JsonConvert.SerializeObject(user.Login),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal("Mail został wysłany do uzytkownika:" + user.Login, json);
        }

        [Fact]
        public async Task Post_ReturnsNotFound_WhenMailProvidedIsInvalid()
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == 3);
            user.Email = "testgmail.com";
            _context.SaveChanges();

            var response = await _client.PostAsync("/auth/reset",
                new StringContent(
                    JsonConvert.SerializeObject(user.Login),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Post_ReturnsNotFound_WhenLoginIsInvalid()
        {
            var user = TestData.CreateTestUser3();

            var response = await _client.PostAsync("/auth/reset",
                new StringContent(
                    JsonConvert.SerializeObject("ZlyLogin1234"),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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