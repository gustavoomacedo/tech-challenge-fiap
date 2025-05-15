using Microsoft.EntityFrameworkCore;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Tests.Integration
{
    public class ContactRepositoryTests : IClassFixture<DbTestFixture>
    {
        private readonly ApplicationDbContext _context;

        public ContactRepositoryTests(DbTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task AdicionarContato_DeveSalvarNoBanco()
        {
            // Arrange
            var contato = new Contact
            {
                Name = "gabriel",
                Email = "gabriel@example.com",
                Telefone = 999998888,
                DDD = 11,
                DataCriacao = DateTime.Now
            };

            // Assert
            var contatoSalvo = await _context.Contact
                .FirstOrDefaultAsync(c => c.Email == "gabriel@example.com");

            if (contatoSalvo == null) {
                // Act
                _context.Contact.Add(contato);
                await _context.SaveChangesAsync();
            }

            Assert.NotNull(contatoSalvo);
            Assert.Equal("gabriel", contatoSalvo.Name);
            Assert.Equal("gabriel@example.com", contatoSalvo.Email);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithContacts()
        {
            // Act
            // Assert
            var contacts = await _context.Contact
                .ToListAsync();

            Assert.True(contacts?.Count > 0);
        }
    }
}
