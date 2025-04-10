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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests.Controllers
{
    public class DevicesControllerTests : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly DevicesController _controller;
        private readonly Mock<IDevicesService> _devicesServiceMock;
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly Mock<ILogger<DevicesController>> _loggerMock;

        private UserDto testUser1;
        private UserDto testUser2;
        private UserDto testUser3;

        private DeviceDto testDevice1;
        private DeviceDto testDevice2;

        public DevicesControllerTests()
        {
            _mapper = GetMapper();
            _devicesServiceMock = new Mock<IDevicesService>();
            _usersServiceMock = new Mock<IUsersService>();
            _loggerMock = new Mock<ILogger<DevicesController>>();
            _controller = new DevicesController(_devicesServiceMock.Object, _usersServiceMock.Object, _loggerMock.Object);

            testUser1 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1());
            testUser2 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser2());
            testUser3 = _mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser3());

            testDevice1 = _mapper.Map<DeviceDto>(TestUtil.TestData.CreateTestDevice1());
            testDevice2 = _mapper.Map<DeviceDto>(TestUtil.TestData.CreateTestDevice2());

            Setup();
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
        public void Get_ReturnsOnlyOwnDevices_WhenUserIsInUserGroup()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser1);
            _devicesServiceMock.Setup(s => s.GetDevicesByUser(testUser1)).Returns(new List<DeviceDto> { testDevice1 });

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var devices = Assert.IsAssignableFrom<List<DeviceDto>>(okResult.Value);
            Assert.Single(devices);
            Assert.Equal(JsonConvert.SerializeObject(new List<DeviceDto> { testDevice1 }), JsonConvert.SerializeObject(devices));
        }

        [Fact]
        public void Get_ReturnsAllDevices_WhenUserIsInAdminGroup()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser3);
            _devicesServiceMock.Setup(s => s.GetDevices()).Returns(new List<DeviceDto> { testDevice1, testDevice2 });

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var devices = Assert.IsAssignableFrom<List<DeviceDto>>(okResult.Value);
            Assert.Equal(2, devices.Count);
            Assert.Equal(JsonConvert.SerializeObject(new List<DeviceDto> { testDevice1, testDevice2 }), JsonConvert.SerializeObject(devices));
        }

        [Fact]
        public void Get_ReturnsForbidden_WhenUserIsNull()
        {
            _devicesServiceMock.Setup(s => s.GetDevices()).Returns(new List<DeviceDto> { testDevice1, testDevice2 });

            var result = _controller.Get();

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsTicket_WhenUserIsInAdminGroup()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser3);
            _devicesServiceMock.Setup(s => s.GetDeviceById(1)).Returns(testDevice1);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var device = Assert.IsAssignableFrom<DeviceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testDevice1), JsonConvert.SerializeObject(device));
        }

        [Fact]
        public void GetById_ReturnsDevice_WhenUserIsDeviceOwner()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser1);
            _devicesServiceMock.Setup(s => s.GetDeviceById(1)).Returns(testDevice1);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var device = Assert.IsAssignableFrom<DeviceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testDevice1), JsonConvert.SerializeObject(device));
        }

        [Fact]
        public void GetById_ReturnsForbidden_WhenUserIsNotDeviceOwner()
        {
            _usersServiceMock.Setup(s => s.GetUserFromToken(It.IsAny<string>())).Returns(testUser2);
            _devicesServiceMock.Setup(s => s.GetDeviceById(1)).Returns(testDevice1);

            var result = _controller.Get(1);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsForbidden_WhenUserIsNull()
        {
            _devicesServiceMock.Setup(s => s.GetDeviceById(1)).Returns(testDevice1);

            var result = _controller.Get(1);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedDevice()
        {
            _devicesServiceMock.Setup(s => s.CreateDevice(testDevice1)).Returns(testDevice1);

            var result = _controller.Post(testDevice1);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var device = Assert.IsAssignableFrom<DeviceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testDevice1), JsonConvert.SerializeObject(device));
        }

        [Fact]
        public void Patch_ReturnsUpdatedDevice_WhenDeviceExists()
        {
            _devicesServiceMock.Setup(s => s.UpdateDevice(testDevice1)).Returns(testDevice1);

            var result = _controller.Patch(testDevice1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var device = Assert.IsAssignableFrom<DeviceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testDevice1), JsonConvert.SerializeObject(device));
        }

        [Fact]
        public void Patch_ReturnsNotFound_WhenDeviceDoesNotExist()
        {
            var result = _controller.Patch(testDevice1);

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
