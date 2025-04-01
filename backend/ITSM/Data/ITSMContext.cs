using ITSM.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace ITSM.Data
{
    public class ITSMContext(DbContextOptions<ITSMContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Device> Devices { get; set; }

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
            modelBuilder.Entity<Device>().HasData(
                 new Device
                 {
                     Id = 1,  // Id urządzenia
                     Name = "Laptop Dell XPS 13",
                     Description = "Laptop ultrabook, 13 cali, i7, 16GB RAM",
                     AcquisitionDate = new DateTime(2023, 1, 15),  // Data zakupu
                     DepreciationDate = new DateTime(2025, 1, 15),  // Data amortyzacji
                     UserId = 1,  // Powiązanie z użytkownikiem
                     Status = "Active"
                 },
                 new Device
                 {
                     Id = 2,  // Id urządzenia
                     Name = "Smartphone Samsung Galaxy S21",
                     Description = "Smartphone z ekranem 6.2 cala, 8GB RAM",
                     AcquisitionDate = new DateTime(2023, 3, 10),
                     DepreciationDate = new DateTime(2025, 3, 10),
                     UserId = 2,
                     Status = "Active"
                 },
                 new Device
                 {
                     Id = 3,  // Id urządzenia
                     Name = "Monitor LG 27",
                     Description = "Monitor 27 cali, 4K",
                     AcquisitionDate = new DateTime(2022, 6, 30),
                     DepreciationDate = new DateTime(2024, 6, 30),
                     UserId = 1,
                     Status = "Active"
                 },
                 new Device
                 {
                     Id = 4,  // Id urządzenia
                     Name = "Printer HP LaserJet Pro",
                     Description = "Drukarka laserowa, czarno-biała",
                     AcquisitionDate = new DateTime(2021, 11, 20),
                     DepreciationDate = new DateTime(2023, 11, 20),
                     UserId = 2,
                     Status = "Inactive"
                 },
                 new Device
                 {
                     Id = 5,  // Id urządzenia
                     Name = "Tablet iPad Pro 12.9",
                     Description = "Tablet 12.9 cala, 256GB, iOS",
                     AcquisitionDate = new DateTime(2022, 9, 5),
                     DepreciationDate = new DateTime(2024, 9, 5),
                     UserId = 1,
                     Status = "Active"
                 }
            );
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Requester)
                .WithMany()
                .HasForeignKey(t => t.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    Name = "Issue with login",
                    Description = "User is unable to log into the system. Error message: 'Invalid credentials.'",
                    CreationDate = new DateTime(2025, 4, 1),
                    SolutionDate = null,
                    SolutionDescription = null,
                    Priority = 2,
                    Type = "Bug",
                    Status = "Open",
                    ServiceId = 1, // Przykład: pierwsza usługa
                    RequesterId = 1, // Przykład: pierwszy użytkownik zgłaszający
                    AssigneeId = 2, // Przykład: drugi użytkownik przypisany
                },
                new Ticket
                {
                    Id = 2,
                    Name = "System performance issue",
                    Description = "The application is lagging when loading the dashboard.",
                    CreationDate = new DateTime(2025, 4, 1),
                    SolutionDate = null,
                    SolutionDescription = null,
                    Priority = 1,
                    Type = "Performance",
                    Status = "In Progress",
                    ServiceId = 2, // Przykład: druga usługa
                    RequesterId = 2, // Przykład: drugi użytkownik zgłaszający
                    AssigneeId = 3, // Przykład: trzeci użytkownik przypisany
                },
                new Ticket
                {
                    Id = 3,
                    Name = "Password reset request",
                    Description = "User has forgotten their password and needs a reset.",
                    CreationDate = new DateTime(2025, 4, 1),
                    SolutionDate = new DateTime(2025, 4, 2),
                    SolutionDescription = "Password was successfully reset and communicated to the user.",
                    Priority = 3,
                    Type = "Support",
                    Status = "Closed",
                    ServiceId = 3, // Przykład: trzecia usługa
                    RequesterId = 3, // Przykład: trzeci użytkownik zgłaszający
                    AssigneeId = 1, // Przykład: pierwszy użytkownik przypisany
                },
                new Ticket
                {
                    Id = 4,
                    Name = "New feature request",
                    Description = "A user requests the addition of a dark mode feature in the app.",
                    CreationDate = new DateTime(2025, 4, 1),
                    SolutionDate = null,
                    SolutionDescription = null,
                    Priority = 4,
                    Type = "Feature Request",
                    Status = "Open",
                    ServiceId = 1, // Przykład: pierwsza usługa
                    RequesterId = 4, // Przykład: czwarty użytkownik zgłaszający
                    AssigneeId = 3, // Przykład: trzeci użytkownik przypisany
                },
                new Ticket
                {
                    Id = 5,
                    Name = "Error while updating profile",
                    Description = "User is receiving an error when trying to update their profile information.",
                    CreationDate = new DateTime(2025, 4, 1),
                    SolutionDate = new DateTime(2025, 4, 3),
                    SolutionDescription = "The issue was resolved by fixing the validation error in the form.",
                    Priority = 2,
                    Type = "Bug",
                    Status = "Closed",
                    ServiceId = 2, // Przykład: druga usługa
                    RequesterId = 5, // Przykład: piąty użytkownik zgłaszający
                    AssigneeId = 2, // Przykład: drugi użytkownik przypisany
                }
            );
        }
    }
}
