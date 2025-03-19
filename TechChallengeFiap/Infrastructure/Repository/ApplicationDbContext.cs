using Microsoft.EntityFrameworkCore;
using TechChallengeFiap.Infrastructure.Configuration;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }


        public DbSet<ContactDto> Contact { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactConfiguration).Assembly);
        }
    }
}