
using Practice.Commands;
using Practice.Data;
using Practice.Models;
using Microsoft.EntityFrameworkCore;
using Practice.Responses;
namespace Practice.Repositories
{
    public class RegisterRepo
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public RegisterRepo(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<UserResponse> GetUserAsync(RegisterUserCommand request)
        {
            var use =
            await _context.Users
          .FirstOrDefaultAsync(
                    x => x.Username ==
                    request.Username);
           var use2 = await _context.Users
          .FirstOrDefaultAsync(
                    x => x.Email ==
                    request.Email);

            if (use != null||use2!=null)
            {
                return new UserResponse
                {
                    Message = "email or username already exists",
                    Success =0
                };
            }
            
            var user = new User
            {

                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                PasswordHash =
              BCrypt.Net.BCrypt.HashPassword(
                  request.Password),
                Address = request.Address,


            };
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new UserResponse { 

                    Message="Internal database error",
                    Success=2
                };
            }
            return new UserResponse
            {

                Message = "User registered succesfully",
                Success = 1
            };

        }
    }
}
