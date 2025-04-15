using AutoMapper;
using ITSM.Data;
using ITSM.Dto;

namespace ITSM.Services
{
    public interface IUsersService
    {
        public List<UserDto> GetUsers();
        public UserDto? GetUserById(int id);
        public UserDto? GetUserByLogin(string login);
        public bool Authenticate(string login, string password);
        public UserDto CreateUser(CreateUserDto userDto);
        public UserDto? UpdateUser(UserDto userDto);
        public void UpdateUserPassword(int id, String Password);
        public void DeleteUser(int id);
        public UserDto? GetUserFromToken(string token);
        public byte[] AllUsersReport();
    }
}
