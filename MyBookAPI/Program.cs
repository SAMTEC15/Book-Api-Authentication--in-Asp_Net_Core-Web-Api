using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBook.Application.Implementations;
using MyBook.Application.Interfaces;
using MyBook.Common.GlobalException;
using MyBook.Common.Utilities;
using MyBook.Domain.Models;
using MyBook.Persistence;
using MyBook.Persistence.Repositories;
using MyBook.Persistence.Repositories.Implementations;
using MyBook.Persistence.Repositories.Interfaces;
using MyBook.Persistence.Seeder;
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

//Registration of all class in the project
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



// Config Identity
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequiredLength = 6;
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.SignIn.RequireConfirmedEmail = false;
//});



//Registration of Idenity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddSingleton<TokenValidationParameters>();

//Token validation parameters
var tokenValidationParameters = new TokenValidationParameters()
{  

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],

    ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
    ValidateAudience = true,

    ValidateActor = true,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero

    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]))
};

// This service was injected in the authenticationService Class this is to help in Checking JWT token format 
builder.Services.AddSingleton(tokenValidationParameters);

//Registrating JWT
builder.Services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
});
builder.Services.AddAuthorization();
//builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "my_book", Version = "v1" });
});

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

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    AddDbInitializer.SeedUserRoles(serviceProvider);
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
