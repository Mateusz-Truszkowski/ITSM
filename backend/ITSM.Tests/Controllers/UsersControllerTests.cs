using AutoMapper;
using ITSM.Controllers;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace ITSM.Tests.Controllers
{
    public class UsersControllerTests : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly UsersController _controller;
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;

        private UserDto testUser1;
        private UserDto testUser2;
        private UserDto testUser3;

        public UsersControllerTests()
        {
            _mapper = GetMapper();
            _usersServiceMock = new Mock<IUsersService>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_loggerMock.Object, _usersServiceMock.Object);

            testUser1 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1());
            testUser2 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser2());
            testUser3 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser3());

            Setup();
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto,  User>();
                cfg.CreateMap<User, CreateUserDto>();
                cfg.CreateMap<CreateUserDto, User>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public void Get_ReturnsOnlyOwnUser_WhenUserIsInUsersGroup()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser1);
            _usersServiceMock.Setup(s => s.GetUserById(1)).Returns(testUser1);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);
            Assert.Single(users);
            Assert.Equal(JsonConvert.SerializeObject(new List<UserDto> { testUser1 }), JsonConvert.SerializeObject(users));
        }

        [Fact]
        public void Get_ReturnsAllUsers_WhenUserIsInAdminGroup()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser3);
            _usersServiceMock.Setup(s => s.GetUsers()).Returns(new List<UserDto>{ testUser1, testUser2, testUser3 });

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);
            Assert.Equal(3, users.Count);
            Assert.Equal(JsonConvert.SerializeObject(new List<UserDto> { testUser1, testUser2, testUser3 }), JsonConvert.SerializeObject(users));
        }

        [Fact]
        public void Get_ReturnsForbidden_WhenUserIsNull()
        {
            var result = _controller.Get();

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsUser_WhenUserIsInAdminGroup()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser3);
            _usersServiceMock.Setup(s => s.GetUserById(1)).Returns(testUser1);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testUser1), JsonConvert.SerializeObject(user));
        }

        [Fact]
        public void GetById_ReturnsUser_WhenUserIsRequestingTheirOwnAccount()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser1);
            _usersServiceMock.Setup(s => s.GetUserById(1)).Returns(testUser1);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testUser1), JsonConvert.SerializeObject(user));
        }

        [Fact]
        public void GetById_ReturnsForbidden_WhenUserIsRequestingNotTheirOwnAccount()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser2);
            _usersServiceMock.Setup(s => s.GetUserById(1)).Returns(testUser1);

            var result = _controller.Get(1);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsForbidden_WhenUserIsNull()
        {
            var result = _controller.Get(1);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedUser()
        {
            var user = _mapper.Map<CreateUserDto>(TestUtil.TestData.CreateTestUser1());
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser3);
            _usersServiceMock.Setup(s => s.CreateUser(user)).Returns(testUser1);

            var result = _controller.Post(user);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdUser = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testUser1), JsonConvert.SerializeObject(createdUser));
        }

        [Fact]
        public void Patch_ReturnsUpdatedUser_WhenUserExists()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser3);
            _usersServiceMock.Setup(s => s.UpdateUser(testUser1)).Returns(testUser1);

            var result = _controller.Patch(testUser1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var createdUser = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testUser1), JsonConvert.SerializeObject(createdUser));
        }

        [Fact]
        public void Patch_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var result = _controller.Patch(testUser1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
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
