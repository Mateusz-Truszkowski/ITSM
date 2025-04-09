using ITSM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests.TestUtil
{
    public class TestData
    {
        public static User CreateTestUser()
        {
            return new User
            {
                Id = 1,
                Login = "jdoe",
                Name = "John",
                Surname = "Doe",
                Email = "j.doe@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreationDate = DateTime.Parse("09.04.2025 14:30:00"),
                Group = "IT",
                Occupation = "Developer",
                Status = "Active"
            };
        }

        public static Ticket CreateTestTicket()
        {
            return new Ticket
            {
                Id = 1,
                Name = "Issue with login",
                Description = "User is unable to log into the system. Error message: 'Invalid credentials.'",
                CreationDate = DateTime.Parse("09.04.2025 14:30:00"),
                SolutionDate = null,
                SolutionDescription = null,
                Priority = 2,
                Type = "Bug",
                Status = "Open",
                ServiceId = 1,
                RequesterId = 1,
                AssigneeId = 2,
            };
        }

        public static Device CreateTestDevice()
        {
            return new Device
            {
                Id = 1,
                Name = "Laptop Dell XPS 13",
                Description = "Laptop ultrabook, 13 cali, i7, 16GB RAM",
                AcquisitionDate = new DateTime(2023, 1, 15),
                DepreciationDate = new DateTime(2025, 1, 15),
                UserId = 1,
                Status = "Active"
            };
        }
    }
}
