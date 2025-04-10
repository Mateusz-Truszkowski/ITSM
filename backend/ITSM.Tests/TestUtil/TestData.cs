using ITSM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ITSM.Tests.TestUtil
{
    public class TestData
    {
        public static User CreateTestUser1()
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
                Group = "User",
                Occupation = "Developer",
                Status = "Active"
            };
        }

        public static User CreateTestUser2()
        {
            return new User
            {
                Id = 2,
                Login = "asmith",
                Name = "Alice",
                Surname = "Smith",
                Email = "asmith@example.com",
                Password = "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni",
                CreationDate = new DateTime(2022, 12, 15),
                Group = "User",
                Occupation = "Product Manager",
                Status = "Active"
            };
        }

        public static User CreateTestUser3()
        {
            return new User
            {
                Id = 3,
                Login = "bjohnson",
                Name = "Bob",
                Surname = "Johnson",
                Email = "bjohnson@example.com",
                Password = "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni",
                CreationDate = new DateTime(2023, 5, 10),
                Group = "Admin",
                Occupation = "QA Engineer",
                Status = "Inactive"
            };
        }

        public static User CreateTestUser4()
        {
            return new User
            {
                Id = 4,
                Login = "klane",
                Name = "Kate",
                Surname = "Lane",
                Email = "klane@example.com",
                Password = "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni",
                CreationDate = new DateTime(2022, 8, 1),
                Group = "User",
                Occupation = "UX Designer",
                Status = "Active"
            };
        }

        public static Ticket CreateTestTicket1()
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

        public static Ticket CreateTestTicket2()
        {
            return new Ticket
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
                ServiceId = 2,
                RequesterId = 3,
                AssigneeId = 3,
            };
        }

        public static Ticket CreateTestTicket3()
        {
            return new Ticket
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
                ServiceId = 3,
                RequesterId = 3,
                AssigneeId = 1,
            };
        }

        public static Device CreateTestDevice1()
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

        public static Device CreateTestDevice2()
        {
            return new Device
            {
                Id = 2,
                Name = "Smartphone Samsung Galaxy S21",
                Description = "Smartphone z ekranem 6.2 cala, 8GB RAM",
                AcquisitionDate = new DateTime(2023, 3, 10),
                DepreciationDate = new DateTime(2025, 3, 10),
                UserId = 3,
                Status = "Active"
            };
        }

        public static Service CreateTestService()
        {
            return new Service
            {
                Id = 1,
                Name = "Web Hosting",
                Description = "Providing reliable and fast web hosting services.",
                ContractingDate = new DateTime(2023, 9, 1),
                Status = "Active",
                SLA = 99
            };
        }
    }
}
