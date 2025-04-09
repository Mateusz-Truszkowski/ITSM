using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITSM.Tests.Services
{
    public class DevicesServiceTests
    {
        private ITSMContext GetDbContext(bool empty = false)
        {
            var options = new DbContextOptionsBuilder<ITSMContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ITSMContext(options);

            if (!empty)
            {
                context.Devices.AddRange(
                    TestUtil.TestData.CreateTestDevice1(),
                    TestUtil.TestData.CreateTestDevice2()
                );

                context.SaveChanges();
            }

            return context;
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
        public void GetDevices_ReturnsListOfDevices()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);

            var result = service.GetDevices();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetDeviceById_ReturnsDevice_WhenDeviceExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);

            var result = service.GetDeviceById(1);

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<DeviceDto>(TestUtil.TestData.CreateTestDevice1())), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetDeviceById_ReturnsNull_WhenDeviceDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);

            var result = service.GetDeviceById(1);

            Assert.Null(result);
        }

        [Fact]
        public void CreateDevice_CreatesDevice_WhenValidDeviceDtoProvided()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);
            var device = mapper.Map<DeviceDto>(TestUtil.TestData.CreateTestDevice1());

            var result = service.CreateDevice(device);

            Assert.Equal(JsonConvert.SerializeObject(device), JsonConvert.SerializeObject(result));
            var createdDevice = mapper.Map<DeviceDto>(context.Devices.FirstOrDefault(x => x.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(device), JsonConvert.SerializeObject(createdDevice));
        }

        [Fact]
        public void UpdateDevice_UpdatesDevice_WhenDeviceExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);
            var device = mapper.Map<DeviceDto>(TestUtil.TestData.CreateTestDevice1());
            device.Description = "updated";

            var result = service.UpdateDevice(device);

            Assert.Equal(JsonConvert.SerializeObject(device), JsonConvert.SerializeObject(result));
            var updatedDevice = mapper.Map<DeviceDto>(context.Devices.FirstOrDefault(x => x.Id == 1));
            Assert.Equal(JsonConvert.SerializeObject(device), JsonConvert.SerializeObject(updatedDevice));
        }

        [Fact]
        public void UpdateDevice_ReturnsNull_WhenDeviceDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);
            var device = mapper.Map<DeviceDto>(TestUtil.TestData.CreateTestDevice1());

            var result = service.UpdateDevice(device);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteDevice_DeletesDevice_WhenDeviceExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);

            service.DeleteDevice(1);

            var deletedDevice = context.Devices.FirstOrDefault(d => d.Id == 1);
            Assert.Null(deletedDevice);
        }

        [Fact]
        public void GetDevicesByUser_ReturnsListOfDevices_WhenUserHaveDevices()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);

            var result = service.GetDevicesByUser(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1()));

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void GetDevicesByUser_ReturnsEmptyList_WhenUserDoesNotHaveDevices()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new DevicesService(context, mapper);

            var result = service.GetDevicesByUser(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser2()));

            Assert.Empty(result);
        }
    }
}
