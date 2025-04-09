using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests.Services
{
    public class UsersServiceTests
    {
        private ITSMContext GetDbContext(bool empty = false)
        {
            var options = new DbContextOptionsBuilder<ITSMContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unikalna baza dla każdego testu
                .Options;

            var context = new ITSMContext(options);

            if (!empty)
            {
                context.Users.AddRange(
                    TestUtil.TestData.CreateTestUser()
                );

                context.SaveChanges();
            }

            return context;
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<CreateUserDto, User>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public void GetUsers_ReturnsListOfUsers()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.GetUsers();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetUserById_ReturnsUser_WhenUserExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.GetUserById(1);

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser())), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetUsersById_ReturnsNull_WhenUserDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.GetUserById(1);

            Assert.Null(result);
        }
    }
}
