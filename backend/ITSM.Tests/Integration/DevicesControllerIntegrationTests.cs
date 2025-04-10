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
    public class DevicesControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JwtTokenService _tokenService;
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        private DeviceDto testDevice1;
        private DeviceDto testDevice2;
        private DeviceDto testDevice3;

        public DevicesControllerIntegrationTests(MyWebApplicationFactory factory)
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

            testDevice1 = _mapper.Map<DeviceDto>(TestData.CreateTestDevice1());
            testDevice2 = _mapper.Map<DeviceDto>(TestData.CreateTestDevice2());
            testDevice3 = _mapper.Map<DeviceDto>(TestData.CreateTestDevice3());
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Device, DeviceDto>();
                cfg.CreateMap<DeviceDto, Device>();
                cfg.CreateMap<User, UserDto>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task Get_ReturnsListOfDevices_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/devices");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<DeviceDto> { testDevice1, testDevice2 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Get_ReturnsOwnedDevices_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/devices");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(new List<DeviceDto> { testDevice1 }).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task Get_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.GetAsync("/devices");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsDevice_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/devices/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testDevice1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsOwnedDevice_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/devices/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testDevice1).ToLower(), json.ToLower());
        }

        [Fact]
        public async Task GetById_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/devices/2");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.GetAsync("/devices/2");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreatesDevice_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(testDevice3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testDevice3).ToLower(), json.ToLower());
            var createdDevice = _mapper.Map<DeviceDto>(_context.Devices.FirstOrDefault(d => d.Id == 3));
            Assert.Equal(JsonConvert.SerializeObject(testDevice3), JsonConvert.SerializeObject(createdDevice));
        }

        [Fact]
        public async Task Post_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(testDevice3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.PostAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(testDevice3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_UpdatesDevice_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);
            var requestDevice = testDevice3;
            requestDevice.Id = 2;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(requestDevice),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(testDevice3).ToLower(), json.ToLower());
            var updatedDevice = _mapper.Map<DeviceDto>(_context.Devices.FirstOrDefault(d => d.Id == 2));
            Assert.Equal(JsonConvert.SerializeObject(requestDevice), JsonConvert.SerializeObject(updatedDevice));
        }

        [Fact]
        public async Task Patch_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);
            var requestDevice = testDevice3;
            requestDevice.Id = 2;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(requestDevice),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var requestDevice = testDevice3;
            requestDevice.Id = 2;

            var response = await _client.PatchAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(requestDevice),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsNotFound_WhenDeviceDoesNotExist()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync("/devices",
                new StringContent(
                    JsonConvert.SerializeObject(testDevice3),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletesDevice_WhenUserRoleIsAdmin()
        {
            var user = TestData.CreateTestUser3();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/devices/2");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var deletedDevice = _context.Devices.FirstOrDefault(d => d.Id == 2);
            Assert.Null(deletedDevice);
        }

        [Fact]
        public async Task Delete_ReturnsForbidden_WhenUserRoleIsUser()
        {
            var user = TestData.CreateTestUser1();
            var token = _tokenService.GenerateToken(user.Login, user.Group);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/devices/2");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsUnauthorized_WhenNoTokenProvided()
        {
            var response = await _client.DeleteAsync("/devices/2");

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

            _context.SaveChanges();
        }
    }
}
