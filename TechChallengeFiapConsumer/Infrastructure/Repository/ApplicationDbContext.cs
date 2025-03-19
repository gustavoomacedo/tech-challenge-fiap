using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechChallengeFiapConsumerAdd.Infrastructure.Configuration;
using TechChallengeFiapConsumerAdd.Models;


namespace TechChallengeFiapConsumerAdd.Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<ContactDto> Contact { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactConfiguration).Assembly);
        }
    }
}