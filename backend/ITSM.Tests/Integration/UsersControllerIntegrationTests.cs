using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using ITSM.Tests.TestUtil;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ITSM.Tests.Integration
{
    [Collection("IntegrationTests")]
    public class UsersControllerIntegrationTests : IClassFixture<MyWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JwtTokenService _tokenService;
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        private UserDto testUser1;
        private UserDto testUser2;
        private UserDto testUser3;
        private CreateUserDto testUser4;

        public UsersControllerIntegrationTests(MyWebApplicationFactory factory)
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
        public async Task Get_ReturnsAllUsers_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<UserDto> { testUser1, testUser2, testUser3 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Get_ReturnsCurrentUser_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<UserDto> { testUser1 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/users/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testUser1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsCurrentUser_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/users/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testUser1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/users/2");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.GetAsync("/users/2");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/users/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreatesUser_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(testUser4),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(_mapper.Map<UserDto>(testUser4)).ToLower(), json.ToLower());
            var createdUser = _mapper.Map<UserDto>(_context.Users.FirstOrDefault(u => u.Id == 4));
            Assert.Equal(JsonConvert.SerializeObject(_mapper.Map<UserDto>(testUser4)).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Post_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(testUser4),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.PostAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(testUser4),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_UpdatesUser_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);
            var requestUser = _mapper.Map<UserDto>( testUser4 );
            requestUser.Id = 1;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(requestUser),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(requestUser).ToLower(), json.ToLower());
            var createdUser = _mapper.Map<UserDto>(_context.Users.FirstOrDefault(u => u.Id == 4));
            Assert.Equal(JsonConvert.SerializeObject(requestUser).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Patch_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser2();
            var token = _tokenService.GenerateToken(user.Login, user.Group);
            var requestUser = _mapper.Map<UserDto>(testUser4);
            requestUser.Id = 1;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(requestUser),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsUnathorized_WhenNoTokenProvided()
        {
            var response = await _client.PatchAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(testUser1),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/users",
                new StringContent(
                    JsonConvert.SerializeObject(testUser4),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletesUser_WhenUserExists()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/users/1");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var deletedUser = _context.Users.FirstOrDefault(u => u.Id == 1);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task Delete_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser2();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/users/1");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.DeleteAsync("/users/1");

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
