using AutoMapper;
using ITSM.Controllers;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace ITSM.Tests.Controllers
{
    public class ServicesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ServicesController _controller;
        private readonly Mock<IServicesService> _servicesServiceMock;
        private readonly Mock<ILogger<ServicesController>> _loggerMock;

        private ServiceDto testService;

        public ServicesControllerTests()
        {
            _mapper = GetMapper();
            _servicesServiceMock = new Mock<IServicesService>();
            _loggerMock = new Mock<ILogger<ServicesController>>();
            _controller = new ServicesController(_loggerMock.Object, _servicesServiceMock.Object);

            testService = _mapper.Map<ServiceDto>(TestUtil.TestData.CreateTestService1());
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
        public void Get_ReturnsListOfServices()
        {
            _servicesServiceMock.Setup(s => s.GetServices()).Returns(new List<ServiceDto> { testService });

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var services = Assert.IsAssignableFrom<List<ServiceDto>>(okResult.Value);
            Assert.Single(services);
            Assert.Equal(JsonConvert.SerializeObject(new List<ServiceDto> { testService }), JsonConvert.SerializeObject(services));
        }

        [Fact]
        public void GetById_ReturnsService_WhenServiceExists()
        {
            _servicesServiceMock.Setup(s => s.GetService(1)).Returns(testService);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var service = Assert.IsAssignableFrom<ServiceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testService), JsonConvert.SerializeObject(service));
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var result = _controller.Get(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedService()
        {
            _servicesServiceMock.Setup(s => s.CreateService(testService)).Returns(testService);

            var result = _controller.Post(testService);

            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var service = Assert.IsAssignableFrom<ServiceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testService), JsonConvert.SerializeObject(service));
        }

        [Fact]
        public void Patch_ReturnsUpdatedService_WhenServiceExists()
        {
            _servicesServiceMock.Setup(s => s.UpdateService(testService)).Returns(testService);

            var result = _controller.Patch(testService);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var service = Assert.IsAssignableFrom<ServiceDto>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(testService), JsonConvert.SerializeObject(service));
        }

        [Fact]
        public void Patch_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var result = _controller.Patch(testService);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
