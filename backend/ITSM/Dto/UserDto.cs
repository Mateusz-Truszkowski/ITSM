
namespace ITSM.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public required string Group { get; set; }
        public required string Occupation { get; set; }
        public required string Status { get; set; }
    }

    public class CreateUserDto
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public required string Group { get; set; }
        public required string Occupation { get; set; }
        public required string Status { get; set; }
    }
}
