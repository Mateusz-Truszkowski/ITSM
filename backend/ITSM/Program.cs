using ITSM.Data;
using ITSM.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<ServicesService>();
builder.Services.AddScoped<DevicesService>();
builder.Services.AddScoped<TicketsService>();
builder.Services.AddControllers();

builder.Services.AddDbContext<ITSMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ITSM_DbCS")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ITSMContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error has occured while migrating the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
