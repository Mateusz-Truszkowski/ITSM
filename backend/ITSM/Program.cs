using ITSM.Data;
using ITSM.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("Missing JWT key in configuration (Jwt:SecretKey)");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Zezwala na wszystkie �r�d�a
              .AllowAnyHeader()  // Zezwala na wszystkie nag��wki
              .AllowAnyMethod(); // Zezwala na wszystkie metody (GET, POST, itp.)
    });
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IDevicesService, DevicesService>();
builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddControllers();

if (builder.Environment.EnvironmentName == "Development")
{
    builder.Services.AddDbContext<ITSMContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ITSM_DbCS")));
}

var app = builder.Build();

app.UseCors("AllowAllOrigins");

if (app.Environment.EnvironmentName == "Development")
{
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
}

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
