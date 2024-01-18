using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBook.Application.Implementations;
using MyBook.Application.Interfaces;
using MyBook.Common.GlobalException;
using MyBook.Common.Utilities;
using MyBook.Domain.Models;
using MyBook.Persistence;
using MyBook.Persistence.Repositories;
using MyBook.Persistence.Repositories.Implementations;
using MyBook.Persistence.Repositories.Interfaces;
using Serilog;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddLogging();
var logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/LogInfomation-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorSerice, AuthorService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});
builder.Services.AddAuthorization();
//builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Seeding Roles and SuperAdmin
/*using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    Seeder.SeedRolesAndSuperAdmin(serviceProvider);
}*/

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
