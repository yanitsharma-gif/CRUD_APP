using Microsoft.AspNetCore.Http;
using Practice.Data;
using System.Threading.Tasks;



namespace Practice.Middlewares
{
    public class CustomFactoryMiddleware:IMiddleware
    {
        private readonly ILogger<CustomFactoryMiddleware> _logger;
        private readonly AppDbContext _dbService;

        public CustomFactoryMiddleware(ILogger<CustomFactoryMiddleware>logger,AppDbContext context)
        {
            _logger=logger;
            _dbService = context;
        }
        public async Task InvokeAsync(HttpContext context,RequestDelegate next)
        {
            //Buisness logic inside this blocks 
            _logger.LogInformation("Checking database for banned users...");
            Console.WriteLine("hello i am yanit sharma");
            await next(context);
        }
    }
}
