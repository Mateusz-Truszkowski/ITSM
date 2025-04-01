using backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ITSMContext(DbContextOptions<ITSMContext> options) : DbContext(options)
    {
        //public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<Service> Services { get; set; }
        //public DbSet<Device> Devices { get; set; }

        // Initial database data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Maciej", Email = "ex1@example.com", Password = "1234" },
                new User { Id = 2, Name = "Kacper", Email = "ex2@example.com", Password = "5678" },
                new User { Id = 3, Name = "Oskar", Email = "ex3@example.com", Password = "qwerty" },
                new User { Id = 4, Name = "Klaudiusz", Email = "ex4@example.com", Password = "ZAQ!2wsx" }
                );
        }
    }
}
