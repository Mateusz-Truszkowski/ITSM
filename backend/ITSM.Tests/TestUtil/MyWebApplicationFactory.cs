using ITSM.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ITSM.Tests.TestUtil
{
    public class MyWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<ITSMContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ITSMContext>();

                    context.Database.EnsureCreated();

                    context.Services.RemoveRange(context.Services);
                    context.Users.RemoveRange(context.Users);
                    context.Devices.RemoveRange(context.Devices);
                    context.Tickets.RemoveRange(context.Tickets);

                    context.Services.AddRange(
                        TestData.CreateTestService()
                    );

                    context.Users.AddRange(
                        TestData.CreateTestUser1(),
                        TestData.CreateTestUser2(),
                        TestData.CreateTestUser3()
                    );

                    context.SaveChanges();

                    context.Devices.AddRange(
                        TestData.CreateTestDevice1(),
                        TestData.CreateTestDevice2()
                    );

                    context.Tickets.AddRange(
                        TestData.CreateTestTicket1(),
                        TestData.CreateTestTicket2()
                    );

                    context.SaveChanges();
                }
            });
        }
    }
}
