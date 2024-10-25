using Moq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechChallengeFiap.Controllers;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;

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
        public void ContactService_AddContact_ShouldReturnObject()
        {
            //Arrange
            var contact = _contactFixture.NewContact();
            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(service => service.AddContact(It.IsAny<Contact>())).Returns(contact);
            var contactService = mockContactService.Object;

            //Act
            var result = contactService.AddContact(contact);

            //Assert
            Assert.Equal(contact.Name, result.Name);
        }

        #endregion

        #region ContactController

        [Fact(DisplayName = "Retorna BadRequest quando contato é inválido")]
        [Trait("ContactController", "Validação de Contato")]
        [Trait("Método", "AddContact")]
        [Trait("Resultado", "BadRequest")]
        public void AddContact_ReturnsBadRequest_WhenContactIsInvalid()
        {
            // Arrange
            var contact = _contactFixture.NewContact();
            _mockValidator.Setup(v => v.Validate(contact))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                new ValidationFailure("Name", "Name is required") // Exemplo de erro de validação
                }));

            // Act
            var result = _controller.AddContact(contact);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("errors", badRequestResult?.Value?.ToString());
        }

        [Fact(DisplayName = "Retorna Created quando contato é válido")]
        [Trait("ContactController", "Validação de Contato")]
        [Trait("Método", "AddContact")]
        [Trait("Resultado", "Created")]
        public void AddContact_ReturnsCreated_WhenContactIsValid()
        {
            // Arrange
            var contact = _contactFixture.NewContact();
            var createdContact = new Contact() { DDD = contact.DDD, Email = contact.Email, Name = contact.Name, Telefone = contact.Telefone };

            _mockValidator.Setup(v => v.Validate(contact))
                .Returns(new ValidationResult());
            _mockContactService.Setup(s => s.AddContact(contact))
                .Returns(createdContact);

            // Act
            var result = _controller.AddContact(contact);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(createdContact, createdResult.Value);
        }

        [Fact(DisplayName = "Retorna StatusCode 500 quando ocorre exceção")]
        [Trait("ContactController", "Tratamento de Erro")]
        [Trait("Método", "AddContact")]
        [Trait("Resultado", "StatusCode 500")]
        public void AddContact_ReturnsStatusCode500_WhenExceptionIsThrown()
        {
            // Arrange
            var contact = _contactFixture.NewContact();

            _mockValidator.Setup(v => v.Validate(contact))
                .Returns(new ValidationResult());
            _mockContactService.Setup(s => s.AddContact(contact))
                .Throws(new Exception("Erro de banco de dados"));

            // Act
            var result = _controller.AddContact(contact);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains("Não foi possível adicionar esse contato", statusCodeResult?.Value?.ToString());
        }
        #endregion
    }
}
