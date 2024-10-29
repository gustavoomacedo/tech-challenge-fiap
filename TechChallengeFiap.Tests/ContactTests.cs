using Moq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechChallengeFiap.Controllers;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;
using TechChallengeFiap.Infrastructure.DTOs;

namespace TechChallengeFiap.Tests
{
    [Collection(nameof(ContactTestFixtureCollection))]
    public class ContactTests
    {
        private readonly ContactController _controller;
        private readonly Mock<ILogger<ContactController>> _mockLoggerController;
        private readonly Mock<IValidator<Contact>> _mockValidator;
        private readonly Mock<IContactService> _mockContactService;
        private readonly ContactFixture _contactFixture;

        public ContactTests(ContactFixture contactFixture)
        {
            _mockLoggerController = new Mock<ILogger<ContactController>>();
            _mockValidator = new Mock<IValidator<Contact>>();
            _mockContactService = new Mock<IContactService>();
            _contactFixture = contactFixture;
            _controller = new ContactController(_mockLoggerController.Object, _mockValidator.Object, _mockContactService.Object);
        }

        #region ContactService

        [Fact(DisplayName = "Validando se está retornando o Contato.")]
        [Trait("ContactService", "Validando Novo Contato")]
        [Trait("Método", "AddContact")]
        public async Task ContactService_AddContact_ShouldReturnObject()
        {
            //Arrange
            var contact = _contactFixture.NewContact();
            var contactResponse = new ContactResponseDTO()
            {
                DDD = contact.DDD,
                Email = contact.Email,
                Name = contact.Name,
                Telefone = contact.Telefone
            };
            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(service => service.AddContactAsync(It.IsAny<ContactRequestDTO>())).ReturnsAsync(contactResponse);
            var contactService = mockContactService.Object;

            //Act
            var result = await contactService.AddContactAsync(contact);

            //Assert
            Assert.Equal(contact.Name, result.Name);
        }

        #endregion

        #region ContactController

        [Fact(DisplayName = "Retorna Created quando contato é válido")]
        [Trait("ContactController", "Validação de Contato")]
        [Trait("Método", "AddContact")]
        [Trait("Resultado", "Created")]
        public async Task AddContact_ReturnsCreated_WhenContactIsValidAsync()
        {
            // Arrange
            var contact = _contactFixture.NewContact();
            var createdContact = new ContactResponseDTO { DDD = contact.DDD, Email = contact.Email, Name = contact.Name, Telefone = contact.Telefone };

            _mockContactService.Setup(s => s.AddContactAsync(contact)).ReturnsAsync(createdContact); // Retorna uma Task<Contact>

            // Act
            var result = await _controller.AddContact(contact); // Método assíncrono

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(createdContact, createdResult.Value);
        }
        #endregion
    }
}
