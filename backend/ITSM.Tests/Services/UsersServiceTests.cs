using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                    TestUtil.TestData.CreateTestUser1()
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

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1())), JsonConvert.SerializeObject(result));
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

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1())), JsonConvert.SerializeObject(result));
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

            var result = service.CreateUser(mapper.Map<CreateUserDto>(TestUtil.TestData.CreateTestUser1()));

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1())), JsonConvert.SerializeObject(result));

            var createdUser = context.Users.FirstOrDefault(u => u.Id == 1);
            Assert.NotNull(createdUser);
            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1())), JsonConvert.SerializeObject(mapper.Map<UserDto>(createdUser)));
        }

        [Fact]
        public void UpdateUser_UpdatesUser_WhenUserExists()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            var user = mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1());
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

            var user = mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1());
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

        [Fact]
        public void GetUserFromToken_ReturnsUser_WhenValidTokenProvided()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqZG9lIiwianRpIjoiNWQ4MDk5MWEtMDYxMi00ODkxLTk4NTctYjg4ZDJhMjQyODIyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NDQyMjIwMjEsImlzcyI6Ikl0c20iLCJhdWQiOiJJdHNtVXNlcnMifQ.2RpLwf1G2HVdaeXJMKiw5nA9uwYDsdwmwCT-H6XAXWA";

            var result = mapper.Map<UserDto>(service.GetUserFromToken(token));

            Assert.Equal(JsonConvert.SerializeObject(mapper.Map<UserDto>(TestUtil.TestData.CreateTestUser1())), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetUserFromToken_ReturnsNull_WhenInvalidToken()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqZG9lIiwianRpIjoiNWQ4MDk5MWEtDZYxMi00ODkxLTk4NTctYjg4ZDJhMjQyODIyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NDQyMjIwMjEsImlzcyI6Ikl0c20iLCJhdWQiOiJJdHNtVXNlcnMifQ.2RpLwf1G2HVdaeXJMKiw5nA9uwYDsdwmwCT-H6XAXWB";

            var result = mapper.Map<UserDto>(service.GetUserFromToken(token));

            Assert.Null(result);
        }

        [Fact]
        public void UpdateUserPassword_ChangesPassword_WhenUserExsist()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);
            var old_password = context.Users.FirstOrDefault(u => u.Id == 1).Password;

            service.UpdateUserPassword(1, "Test");

            var new_password = context.Users.FirstOrDefault(u => u.Id == 1).Password;
            Assert.NotEqual(old_password, new_password);
        }
        [Fact]
        public void UpdateUserPassword_DoesNothing_WhenUserDoesntExsist()
        {
            var context = GetDbContext();
            var mapper = GetMapper();
            var service = new UsersService(context, mapper);

            service.UpdateUserPassword(2, "Test");
            //Should not throw exception
        }
    }
}
