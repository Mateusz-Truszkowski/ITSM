using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITSM.Tests.Services
{
    public class ServicesServiceTests
    {
        private ITSMContext GetDbContext(bool empty = false)
        {
            var options = new DbContextOptionsBuilder<ITSMContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ITSMContext(options);

            if (!empty)
            {
                context.Services.AddRange(
                    TestUtil.TestData.CreateTestService1()
                );

                context.SaveChanges();
            }

            return context;
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
        public void GetServices_ReturnsListOfServices()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);

            var result = service.GetServices();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetService_ReturnsService_WhenServiceExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);

            var result = service.GetService(1);

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<ServiceDto>(TestUtil.TestData.CreateTestService1())), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetService_ReturnsNull_WhenServiceDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);

            var result = service.GetService(1);

            Assert.Null(result);
        }

        [Fact]
        public void CreateService_CreatesService_WhenValidServiceDtoProvided()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);
            var serviceDto = mapper.Map<ServiceDto>(TestUtil.TestData.CreateTestService1());

            var result = service.CreateService(serviceDto);

            Assert.Equal(JsonConvert.SerializeObject(serviceDto), JsonConvert.SerializeObject(result));
            var createdService = mapper.Map<ServiceDto>(context.Services.FirstOrDefault(s => s.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(createdService), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void UpdateService_UpdatesService_WhenServiceExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);
            var serviceDto = mapper.Map<ServiceDto>(TestUtil.TestData.CreateTestService1());
            serviceDto.Name = "updated";

            var result = service.UpdateService(serviceDto);

            Assert.Equal(JsonConvert.SerializeObject(serviceDto), JsonConvert.SerializeObject(result));
            var updatedService = mapper.Map<ServiceDto>(context.Services.FirstOrDefault(s => s.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(updatedService), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void UpdateService_ReturnsNull_WhenServiceDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);
            var serviceDto = mapper.Map<ServiceDto>(TestUtil.TestData.CreateTestService1());

            var result = service.UpdateService(serviceDto);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteService_DeletesService_WhenServiceExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new ServicesService(context, mapper);

            service.DeleteService(1);

            var deletedService = context.Services.FirstOrDefault(s => s.Id == 1);
            Assert.Null(deletedService);
        }
    }
}
