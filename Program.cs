
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Practice.Configurations;
using Practice.Data;
using Practice.Middlewares;
using Practice.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<JwtService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters =
             JwtTokenValidation.Create(jwtSettings!);
    }
    );
    

 builder.Services.AddAuthorization();
builder.Services.AddTransient<CustomFactoryMiddleware>();


var app = builder.Build();
app.UseMiddleware<CustomFactoryMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(); 

app.Run();
