using ITSM.Entity;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Data
{
    public class ITSMContext(DbContextOptions<ITSMContext> options) : DbContext(options)
    {
        //public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        //public DbSet<Device> Devices { get; set; }

        // Initial database data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { 
                    Id = 1,
                    Login = "jdoe", 
                    Name = "John",
                    Surname = "Doe",
                    Email = "jdoe@example.com",
                    Password = "Password123",
                    CreationDate = new DateTime(2023, 9, 1),
                    Group = "Admin",
                    Occupation = "Software Engineer",
                    Status = "Active"
                },
                new User
                {
                    Id = 2,
                    Login = "asmith",
                    Name = "Alice",
                    Surname = "Smith",
                    Email = "asmith@example.com",
                    Password = "SecurePass456",
                    CreationDate = new DateTime(2022, 12, 15),
                    Group = "User",
                    Occupation = "Product Manager",
                    Status = "Active"
                },
                new User
                {
                    Id = 3,
                    Login = "bjohnson",
                    Name = "Bob",
                    Surname = "Johnson",
                    Email = "bjohnson@example.com",
                    Password = "TestPass789",
                    CreationDate = new DateTime(2023, 5, 10),
                    Group = "Moderator",
                    Occupation = "QA Engineer",
                    Status = "Inactive"
                },
                new User
                {
                    Id = 4,
                    Login = "klane",
                    Name = "Kate",
                    Surname = "Lane",
                    Email = "klane@example.com",
                    Password = "KatePass999",
                    CreationDate = new DateTime(2022, 8, 1),
                    Group = "User",
                    Occupation = "UX Designer",
                    Status = "Active"
                },
                new User
                {
                    Id = 5,
                    Login = "mwhite",
                    Name = "Michael",
                    Surname = "White",
                    Email = "mwhite@example.com",
                    Password = "MikePass111",
                    CreationDate = new DateTime(2023, 3, 20),
                    Group = "Admin",
                    Occupation = "CTO",
                    Status = "Suspended"
                }
            );
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Name = "Web Hosting",
                    Description = "Providing reliable and fast web hosting services.",
                    ContractingDate = new DateTime(2023, 9, 1),
                    Status = "Active",
                    SLA = 99
                },
                new Service
                {
                    Id = 2,
                    Name = "Cloud Storage",
                    Description = "Scalable cloud storage solutions for businesses of all sizes.",
                    ContractingDate = new DateTime(2022, 12, 15),
                    Status = "Active",
                    SLA = 99
                },
                new Service
                {
                    Id = 3,
                    Name = "Email Marketing",
                    Description = "Email marketing campaigns, automation, and reporting.",
                    ContractingDate = new DateTime(2023, 5, 10),
                    Status = "Inactive",
                    SLA = 98
                },
                new Service
                {
                    Id = 4,
                    Name = "Data Backup",
                    Description = "Secure backup and disaster recovery solutions.",
                    ContractingDate = new DateTime(2022, 8, 1),
                    Status = "Active",
                    SLA = 97
                },
                new Service
                {
                    Id = 5,
                    Name = "SEO Optimization",
                    Description = "Search engine optimization to increase website visibility.",
                    ContractingDate = new DateTime(2023, 3, 20),
                    Status = "Active",
                    SLA = 95
                }
            );
        }
    }
}
