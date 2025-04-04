using AutoMapper;
using BCrypt.Net;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;

namespace ITSM.Services
{
    public class UsersService
    {
        private readonly IMapper _mapper;
        private readonly ITSMContext _context;

        public UsersService(ITSMContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public List<UserDto> GetUsers()
        {
            return _context.Users.Select(user => _mapper.Map<UserDto>(user)).ToList();
        }

        public UserDto GetUserById(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).Select(u => _mapper.Map<UserDto>(u)).FirstOrDefault();
            if (user == null)
                return null;
            return user;
        }

        public UserDto GetUserByLogin(string login)
        {
            var user = _context.Users.Where(u => u.Login == login).FirstOrDefault();
            if (user == null)
                return null;
            return _mapper.Map<UserDto>(user);
        }

        public bool Authenticate(string login, string password)
        {
            var user = _context.Users.Where(u => u.Login == login).FirstOrDefault();
            if (user == null)
                return false;
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return false;
            return true;
        }

        public UserDto CreateUser(CreateUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(user);
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            var user = _context.Users.Where(user => user.Id == userDto.Id).FirstOrDefault();
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(userDto.Login))
                user.Login = userDto.Login;

            if (!string.IsNullOrEmpty(userDto.Name))
                user.Name = userDto.Name;

            if (!string.IsNullOrEmpty(userDto.Surname))
                user.Surname = userDto.Surname;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            if (userDto.CreationDate != DateTime.MinValue)
                user.CreationDate = userDto.CreationDate;

            if (!string.IsNullOrEmpty(userDto.Group))
                user.Group = userDto.Group;

            if (!string.IsNullOrEmpty(userDto.Occupation))
                user.Occupation = userDto.Occupation;

            if (!string.IsNullOrEmpty(userDto.Status))
                user.Status = userDto.Status;

            _context.SaveChanges();

            return _mapper.Map<UserDto>(user);
        }

        public void DeleteUser(int id)
        {
            var toDelete = _context.Users.Where(user => user.Id == id).FirstOrDefault();
            if (toDelete != null)
            {
                _context.Users.Remove(toDelete);
                _context.SaveChanges();
            }
        }
    }
}
