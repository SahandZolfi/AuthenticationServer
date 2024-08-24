using AuthenticationServer.API;
using AuthenticationServer.Core.AutoMapperProfiles;
using AuthenticationServer.Core.IRepository;
using AuthenticationServer.Core.IServices;
using AuthenticationServer.Core.Services;
using AuthenticationServer.Persistence.DatabaseContext;
using AuthenticationServer.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//Configuring Logger
builder.Host.UseSerilog();
string currentDirectory = Environment.CurrentDirectory;
Log.Logger = new LoggerConfiguration().WriteTo.File(
    path: currentDirectory + "\\Logs\\Log-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exeption}",
    restrictedToMinimumLevel: LogEventLevel.Information,
    rollingInterval: RollingInterval.Day).CreateLogger();

//Adding AutoMapper Service
builder.Services.AddAutoMapper(typeof(MapperInitializer));

//Adding JWT Auth Service
builder.Services.AddAuthentication();
builder.Services.ConfigureJWT(builder.Configuration);

//Registering Repositories
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFileService, FileService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Bearer token authentication
    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    };
    c.AddSecurityDefinition("jwt_auth", securityDefinition);

    // Make sure swagger UI requires a Bearer token specified
    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Id = "jwt_auth",
            Type = ReferenceType.SecurityScheme
        }
    };
    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
{
    {securityScheme, new string[] { }},
};
    c.AddSecurityRequirement(securityRequirements);
});

//Adding Cores Policies
builder.Services.AddCors(cors => cors.AddPolicy("DefaultPolicy", o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

//Adding DataBase Context
string? connectionString = builder.Configuration.GetConnectionString("DefaultLocalConnectionString");
builder.Services.AddDbContext<DataBaseContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

//Configure Cores
app.UseCors("DefaultPolicy");

//Configure Global Exeption Handling
app.ConfigureExeptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Log.Information("Application Starting");
app.Run();
