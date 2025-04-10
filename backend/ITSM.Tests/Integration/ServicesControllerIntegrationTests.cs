using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using ITSM.Tests.TestUtil;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests.Integration
{
    [Collection("IntegrationTests")]
    public class ServicesControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JwtTokenService _tokenService;
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        private ServiceDto testService1;
        private ServiceDto testService2;
        private ServiceDto testService3;

        public ServicesControllerIntegrationTests(MyWebApplicationFactory factory)
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

            testService1 = _mapper.Map<ServiceDto>(TestData.CreateTestService1());
            testService2 = _mapper.Map<ServiceDto>(TestData.CreateTestService2());
            testService3 = _mapper.Map<ServiceDto>(TestData.CreateTestService3());
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Service, ServiceDto>();
                cfg.CreateMap<ServiceDto, Service>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task Get_ReturnsListOfServices_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/services");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<ServiceDto> { testService1, testService2 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Get_ReturnsListOfServices_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/services");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<ServiceDto> { testService1, testService2 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Get_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.GetAsync("/services");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsService_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/services/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testService1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsService_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/services/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testService1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/services/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.GetAsync("/services/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreatesService_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(testService3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testService3).ToLower(), json.ToLower());
            var createdService = _mapper.Map<ServiceDto>(_context.Services.FirstOrDefault(s => s.Id == 3));
            Assert.Equal(JsonConvert.SerializeObject(testService3), JsonConvert.SerializeObject(createdService));
        }

        [Fact]
        public async Task Post_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(testService3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.PostAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(testService3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_UpdatesService_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);
            var requestService = testService3;
            requestService.Id = 2;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(requestService),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(requestService).ToLower(), json.ToLower());
            var updatedService = _mapper.Map<ServiceDto>(_context.Services.FirstOrDefault(s => s.Id == 2));
            Assert.Equal(JsonConvert.SerializeObject(requestService), JsonConvert.SerializeObject(updatedService));
        }

        [Fact]
        public async Task Patch_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(testService2),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.PatchAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(testService2),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/services",
                new StringContent(
                    JsonConvert.SerializeObject(testService3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletesService_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/services/2");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var deletedService = _context.Services.FirstOrDefault(s => s.Id == 2);
            Assert.Null(deletedService);
        }

        [Fact]
        public async Task Delete_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/services/2");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.DeleteAsync("/services/2");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private void ResetDb()
        {
            _context.Database.EnsureDeleted();
            _context.ChangeTracker.Clear();

            _context.Services.AddRange(
                TestData.CreateTestService1(),
                TestData.CreateTestService2()
            );

            _context.Users.AddRange(
                TestData.CreateTestUser1(),
                TestData.CreateTestUser2(),
                TestData.CreateTestUser3()
            );

            _context.SaveChanges();
        }
    }
}
