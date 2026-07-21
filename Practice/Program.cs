
using System.Text;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Practice;
using Practice.Configurations;
using Practice.Data;
using Practice.Middlewares;
using Practice.Repositories;
using Practice.Services;
using Practice.Validators;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;


var builder = WebApplication.CreateBuilder(args);


var secretsClient = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.USEast1);

var response = await secretsClient.GetSecretValueAsync(new GetSecretValueRequest
{
    SecretId = "my-app-secrets"
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Configuration.AddJsonStream(
    new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response.SecretString))
);
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return new BadRequestObjectResult(new
        {
            Success = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Message = "Validation Failed",
            Errors = errors
        });
    };
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
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
builder.Services.AddScoped<LoginRepo>();
builder.Services.AddScoped<RegisterRepo>();
builder.Services.AddScoped<GetRepo>();
builder.Services.AddScoped<CreateRepo>();
builder.Services.AddScoped<GetAllRepo>();
builder.Services.AddScoped<UpdateRepo>();
builder.Services.AddScoped<DeleteRepo>();
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
                    StatusCode = StatusCodes.Status400BadRequest,
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
app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});
app.UseMiddleware<CustomFactoryMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(); 

app.Run();
