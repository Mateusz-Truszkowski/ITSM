using AutoMapper;
using ClosedXML.Excel;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using System.IdentityModel.Tokens.Jwt;

namespace ITSM.Services
{
    public class UsersService : IUsersService
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

        public UserDto? GetUserById(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).Select(u => _mapper.Map<UserDto>(u)).FirstOrDefault();
            if (user == null)
                return null;
            return user;
        }

        public UserDto? GetUserByLogin(string login)
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

        public UserDto? UpdateUser(UserDto userDto)
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
        public void UpdateUserPassword(int id, String Password)
        {
            
            var user = _context.Users.Where(user => user.Id == id).FirstOrDefault();
            if (user != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                _context.SaveChanges();
            }
        }

        public UserDto? GetUserFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                var login = jsonToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                if (login == null)
                    return null;

                return GetUserByLogin(login);
            } 
            catch
            {
                return null;
            }
        }
        public byte[] AllUsersReport()
        {
            var users = GetUsers();
            

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Login";
            worksheet.Cell(1, 3).Value = "Name";
            worksheet.Cell(1, 4).Value = "Surname";
            worksheet.Cell(1, 5).Value = "Email";
            worksheet.Cell(1, 6).Value = "CreationDate";
            worksheet.Cell(1, 7).Value = "Group";
            worksheet.Cell(1, 8).Value = "Occupation";
            worksheet.Cell(1, 9).Value = "Status";

            // Dodanie danych
            for (int i = 0; i < users.Count; i++)
            {
                var ticket = users[i];
                worksheet.Cell(i + 2, 1).Value = ticket.Id;
                worksheet.Cell(i + 2, 2).Value = ticket.Login;
                worksheet.Cell(i + 2, 3).Value = ticket.Name;
                worksheet.Cell(i + 2, 4).Value = ticket.Surname;
                worksheet.Cell(i + 2, 5).Value = ticket.Email;
                worksheet.Cell(i + 2, 6).Value = ticket.CreationDate;
                worksheet.Cell(i + 2, 7).Value = ticket.Group;
                worksheet.Cell(i + 2, 8).Value = ticket.Occupation;
                worksheet.Cell(i + 2, 9).Value = ticket.Status;
            }


            using var stream = new System.IO.MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
