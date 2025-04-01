namespace ITSM.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public string Group { get; set; }
        public string Occupation { get; set; }
        public string Status { get; set; }
    }

    public class CreateUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public string Group { get; set; }
        public string Occupation { get; set; }
        public string Status { get; set; }
    }
}
