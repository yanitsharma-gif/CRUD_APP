
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Practice;
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




builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Practice API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token (without the word 'Bearer')"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});






builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters =
             JwtTokenValidation.Create(jwtSettings!);



        options.Events = new JwtBearerEvents
        {


            OnChallenge = async context =>
            {
                // Stop the default behavior (empty 401 with WWW-Authenticate header)
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    success = false,
                    message = "Token required. Please sign in.",
                    statusCode = 401
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
            ,
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    success = false,
                    message = "You do not have access to perform this action.",
                    statusCode = 403
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
        };
    });
    

 builder.Services.AddAuthorization();
builder.Services.AddTransient<CustomFactoryMiddleware>();


var app = builder.Build();
app.UseMiddleware<CustomFactoryMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<chatHub>("/chathub");
app.UseSwagger();
app.UseSwaggerUI(); 

app.Run();
