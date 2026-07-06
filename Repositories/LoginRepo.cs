using Castle.Core.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Practice.Commands;
using Practice.Data;
using Practice.Models;

namespace Practice.Repositories
{
    public class LoginRepo
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public LoginRepo(IConfiguration configuration,AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<User> GetUserAsync(LoginUserCommand request)
        {
            var user =
              await _context.Users
                  .FirstOrDefaultAsync(
                      x => x.Username ==
                      request.Username);

            if (user == null)
            {
                return null;
            }

            var validPassword =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!validPassword)
            {
                return null;
            }
            if (request.secretKey == _configuration["secret"])
            {
                user.role = "admin";
            }
            else
            {
                user.role = "user";
            }
            return user;
        }
    }
}
