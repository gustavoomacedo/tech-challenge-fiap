using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Tests.Integration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ContactDto> Contact { get; set; }
    }

    public class DbTestFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }
        private readonly SqlConnection _connection;

        public DbTestFixture()
        {
            // Conectar ao banco de dados de teste (ou criar um banco em memória)
            var connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=ContactDB;Persist Security Info=True;User ID=sa;Password=12345678;Trust Server Certificate=True";
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_connection)
                .Options;

            Context = new ApplicationDbContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context?.Dispose();
            _connection?.Dispose();
        }
    }

}
