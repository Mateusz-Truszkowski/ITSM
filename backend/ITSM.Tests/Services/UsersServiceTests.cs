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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
                cfg.CreateMap<User,  CreateUserDto>();
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

        [Fact]
        public void GetUserByLogin_ReturnsUser_WhenUserExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.GetUserByLogin("jdoe");

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser())), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetUserByLogin_ReturnsNull_WhenUserDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.GetUserByLogin("jdoe");

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_ReturnsTrue_WhenCredentialsCorrect()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.Authenticate("jdoe", "password123");

            Assert.True(result);
        }

        [Fact]
        public void Authenticate_ReturnsFalse_WhenPasswordIncorrect()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.Authenticate("jdoe", "123");

            Assert.False(result);
        }

        [Fact]
        public void Authenticate_ReturnsFalse_WhenLoginIncorrect()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.Authenticate("jdoe2", "password123");

            Assert.False(result);
        }

        [Fact]
        public void CreateUser_CreatesUser_WhenValidUserDtoIsProvided()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var result = service.CreateUser(mapper.Map<CreateUserDto>(TestUtil.TestData.CreateTestUser()));

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser())), JsonConvert.SerializeObject(result));

            var createdUser = context.Users.FirstOrDefault(u => u.Id == 1);
            Assert.NotNull(createdUser);
            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser())), JsonConvert.SerializeObject(mapper.Map<UserDto>(createdUser)));
        }

        [Fact]
        public void UpdateUser_UpdatesUser_WhenUserExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var user = mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser());
            user.Login = "updated";

            var result = service.UpdateUser(user);

            Assert.Equal(JsonConvert.SerializeObject(user), JsonConvert.SerializeObject(result));

            var updatedUser = context.Users.FirstOrDefault(u => u.Id == 1);
            Assert.Equal(JsonConvert.SerializeObject(user), JsonConvert.SerializeObject(mapper.Map<UserDto>(updatedUser)));
        }

        [Fact]
        public void UpdateUser_ReturnsNull_WhenUserDoesNotExist()
        {
            var context = GetDbContext(true);
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var user = mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser());
            user.Login = "updated";

            var result = service.UpdateUser(user);

            Assert.Null(result);
        }

        [Fact]
        public void DeleteUser_DeletesUser_WhenUserExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            service.DeleteUser(1);

            var deletedUser = context.Users.FirstOrDefault(u => u.Id == 1);
            Assert.Null(deletedUser);
        }
    }
}
