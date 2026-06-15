
using Microsoft.EntityFrameworkCore;
using Practice.Models;

namespace Practice.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;
        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
            : base(options)
        {
            _logger=logger;
        }

        public DbSet<Product> Products { get; set; }

      
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _logger.LogInformation("code for checking uniqueness");
            modelBuilder.Entity<User>(entity =>
            {
                // Only things that CAN'T be done via Data Annotations
                entity.HasIndex(r => r.Email).IsUnique();
                entity.HasIndex(r => r.Username).IsUnique();
            });
        }








    }
}