using backend.Data;
using backend.Dto;
using backend.Entity;

namespace backend.Services
{
    public class UsersService
    {
        ITSMContext _context;

        public UsersService(ITSMContext context) 
        {
            _context = context; 
        }

        public List<UserDto> GetUsers()
        {
            return _context.Users.Select(user => new UserDto { Id = user.Id, Name = user.Name, Email = user.Email}).ToList();
        }

        public UserDto GetUserById(int id)
        {
            return _context.Users.Where(u => u.Id ==  id)
                .Select(u => new UserDto{Id = u.Id, Name = u.Name, Email = u.Email})
                .FirstOrDefault();
        }

        public UserDto CreateUser(CreateUserDto userDto)
        {
            User user = new User { Name = userDto.Name, Email = userDto.Email, Password = userDto.Password };
            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            User user = _context.Users.Where(user => user.Id == userDto.Id).FirstOrDefault();
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(userDto.Name))
                user.Name = userDto.Name;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            _context.SaveChanges();

            return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
        }

        public void DeleteUser(int id)
        {
            User toDelete = _context.Users.Where(user => user.Id == id).FirstOrDefault();
            if (toDelete != null)
            {
                _context.Users.Remove(toDelete);
                _context.SaveChanges();
            }
        }
    }
}
