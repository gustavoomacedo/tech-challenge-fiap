using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechChallengeFiap.Controllers;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;
using TechChallengeFiap.Infrastructure.DTOs;
using TechChallengeFiap.Infrastructure.Services;
using TechChallengeFiap.RabbitMQ;

namespace TechChallengeFiap.Tests
{
    [Collection(nameof(ContactTestFixtureCollection))]
    public class ContactTests
    {
        private readonly ContactController _controller;
        private readonly Mock<ILogger<ContactController>> _mockLoggerController;
        private readonly Mock<IContactService> _mockContactService;
        private readonly ContactFixture _contactFixture;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Mock<IMessagePublisher> _contactMessageMock;
        private readonly ContactService _service;

        public ContactTests(ContactFixture contactFixture)
        {
            _mockLoggerController = new Mock<ILogger<ContactController>>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _contactMessageMock = new Mock<IMessagePublisher>();
            _service = new ContactService(_contactRepositoryMock.Object);
            _mockContactService = new Mock<IContactService>();
            _contactFixture = contactFixture;
            _controller = new ContactController(_mockLoggerController.Object, _mockContactService.Object, _contactMessageMock.Object);
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

        [Fact]
        public async Task ContactService_AddContactAsync_ShouldReturnContactResponseDTO_WhenContactIsValid()
        {
            // Arrange
            var contactDTO = new ContactRequestDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                DDD = 11,
                Telefone = 123456789
            };
            var contactId = 1;
            _contactRepositoryMock.Setup(r => r.Add(It.IsAny<ContactDto>()))
                                  .ReturnsAsync(contactId);

            // Act
            var result = await _service.AddContactAsync(contactDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactDTO.Name, result.Name);
            Assert.Equal(contactDTO.Email, result.Email);
            Assert.Equal(contactDTO.DDD, result.DDD);
            Assert.Equal(contactDTO.Telefone, result.Telefone);
            Assert.Equal(contactId, result.Id);
        }

        [Fact]
        public async Task ContactService_GetAllAsync_ShouldReturnListOfContacts()
        {
            // Arrange
            var contacts = new List<ContactDto>
            {
                new ContactDto { Id = 1, Name = "John Doe", Email = "john@example.com", DDD = 11, Telefone = 123456789 }
            };
            _contactRepositoryMock.Setup(r => r.GetAll())
                                  .ReturnsAsync(contacts);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(contacts.Count, result.Count);
            Assert.Equal(contacts[0].Name, result.First().Name);
        }

        [Fact]
        public async Task ContactService_GetByIdAsync_ShouldReturnContact_WhenContactExists()
        {
            // Arrange
            var contact = new ContactDto { Id = 1, Name = "John Doe", Email = "john@example.com", DDD = 11, Telefone = 123456789 };
            _contactRepositoryMock.Setup(r => r.GetById(contact.Id))
                                  .ReturnsAsync(contact);

            // Act
            var result = await _service.GetByIdAsync(contact.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contact.Name, result.Name);
        }

        [Fact]
        public async Task ContactService_GetByIdAsync_ShouldThrowException_WhenContactDoesNotExist()
        {
            // Arrange
            var contactId = 99;
            _contactRepositoryMock.Setup(r => r.GetById(contactId))
                                  .ReturnsAsync((ContactDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(contactId));
        }

        //[Fact]
        //public async Task ContactService_DeleteContactAsync_ShouldCallRepositoryDelete_WhenCalledWithValidId()
        //{
        //    // Arrange
        //    var contactId = 1;
        //    _contactRepositoryMock.Setup(r => r.Delete(contactId))
        //                          .Returns(Task.CompletedTask);

        //    // Act
        //    await _service.deleteContactAsync(contactId);

        //    // Assert
        //    _contactRepositoryMock.Verify(r => r.Delete(contactId), Times.Once);
        //}

        //[Fact]
        //public async Task ContactService_UpdateContactAsync_ShouldThrowException_WhenContactDoesNotExist()
        //{
        //    // Arrange
        //    var contactUpdate = new ContactUpdateRequestDTO
        //    {
        //        Id = 1,
        //        Name = "John Doe",
        //        Email = "john@example.com",
        //        DDD = 11,
        //        Telefone = 123456789
        //    };
        //    _contactRepositoryMock.Setup(r => r.GetById(contactUpdate.Id))
        //                          .ReturnsAsync((ContactDto)null);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _service.updateContactAsync(contactUpdate));
        //}

        //[Fact]
        //public async Task ContactService_UpdateContactAsync_ShouldUpdateContact_WhenContactExists()
        //{
        //    // Arrange
        //    var contact = new ContactDto { Id = 1, Name = "Old Name", Email = "old@example.com", DDD = 11, Telefone = 123456789 };
        //    var contactUpdate = new ContactUpdateRequestDTO
        //    {
        //        Id = 1,
        //        Name = "New Name",
        //        Email = "new@example.com",
        //        DDD = 11,
        //        Telefone = 987654321
        //    };
        //    _contactRepositoryMock.Setup(r => r.GetById(contactUpdate.Id))
        //                          .ReturnsAsync(contact);
        //    _contactRepositoryMock.Setup(r => r.Update(It.IsAny<ContactDto>()))
        //                          .Returns(Task.CompletedTask);

        //    // Act
        //    await _service.updateContactAsync(contactUpdate);

        //    // Assert
        //    _contactRepositoryMock.Verify(r => r.Update(It.Is<ContactDto>(c =>
        //        c.Name == contactUpdate.Name &&
        //        c.Email == contactUpdate.Email &&
        //        c.DDD == contactUpdate.DDD &&
        //        c.Telefone == contactUpdate.Telefone
        //    )), Times.Once);
        //}

        //[Fact]
        //public async Task ContactService_GetAllContactsByDDDAsync_ShouldReturnContacts_WhenContactsExist()
        //{
        //    // Arrange
        //    var ddd = 11;
        //    var contacts = new List<ContactDto>
        //    {
        //        new ContactDto { Id = 1, Name = "John Doe", Email = "john@example.com", DDD = ddd, Telefone = 123456789 }
        //    };
        //    _contactRepositoryMock.Setup(r => r.GetContactsByDDD(ddd))
        //                          .ReturnsAsync(contacts);

        //    // Act
        //    var result = await _service.GetAllContactsByDDDAsync(ddd);

        //    // Assert
        //    Assert.NotEmpty(result);
        //    Assert.Equal(contacts.Count, result.Count);
        //    Assert.Equal(contacts[0].Name, result.First().Name);
        //}
        //#endregion

        //#region ContactController

        //[Fact(DisplayName = "Retorna Created quando contato é válido")]
        //[Trait("ContactController", "Validação de Contato")]
        //[Trait("Método", "AddContact")]
        //[Trait("Resultado", "Created")]
        //public async Task ContactController_AddContact_ReturnsCreated_WhenContactIsValidAsync()
        //{
        //    // Arrange
        //    var contact = _contactFixture.NewContact();
        //    var createdContact = new ContactResponseDTO { DDD = contact.DDD, Email = contact.Email, Name = contact.Name, Telefone = contact.Telefone };

        //    _mockContactService.Setup(s => s.AddContactAsync(contact)).ReturnsAsync(createdContact); // Retorna uma Task<Contact>

        //    // Act
        //    var result = await _controller.AddContact(contact); // Método assíncrono

        //    // Assert
        //    var createdResult = Assert.IsType<CreatedResult>(result);
        //    Assert.Equal(createdContact, createdResult.Value);
        //}

        //[Fact]
        //public async Task ContactController_AddContact_ReturnsBadRequest_WhenNameIsMissing()
        //{
        //    // Arrange
        //    var contact = new ContactRequestDTO
        //    {
        //        Email = "test@example.com",
        //        DDD = 11,
        //        Telefone = 123456789
        //    };
        //    _controller.ModelState.AddModelError("Name", "Required");

        //    // Act
        //    var result = await _controller.AddContact(contact);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task ContactController_AddContact_ReturnsBadRequest_WhenEmailIsMissing()
        //{
        //    // Arrange
        //    var contact = new ContactRequestDTO
        //    {
        //        Name = "John Doe",
        //        DDD = 11,
        //        Telefone = 123456789
        //    };
        //    _controller.ModelState.AddModelError("Email", "Required");

        //    // Act
        //    var result = await _controller.AddContact(contact);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task ContactController_AddContact_ReturnsBadRequest_WhenDddIsInvalid()
        //{
        //    // Arrange
        //    var contact = new ContactRequestDTO
        //    {
        //        Name = "John Doe",
        //        Email = "test@example.com",
        //        DDD = 0,  // DDD inválido
        //        Telefone = 123456789
        //    };
        //    _controller.ModelState.AddModelError("DDD", "Invalid DDD");

        //    // Act
        //    var result = await _controller.AddContact(contact);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task ContactController_AddContact_ReturnsBadRequest_WhenTelefoneIsInvalid()
        //{
        //    // Arrange
        //    var contact = new ContactRequestDTO
        //    {
        //        Name = "John Doe",
        //        Email = "test@example.com",
        //        DDD = 11,
        //        Telefone = 0  // Telefone inválido
        //    };
        //    _controller.ModelState.AddModelError("Telefone", "Invalid phone number");

        //    // Act
        //    var result = await _controller.AddContact(contact);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task Contacts_ShouldReturnOk_WithListOfContacts()
        //{
        //    // Arrange
        //    var contacts = new List<ContactResponseDTO> { new ContactResponseDTO { Id = 1, Name = "Test", DDD = 11, Email = "test@email.com", Telefone = 55555555, DataCriacao = DateTime.Now } };
        //    _mockContactService.Setup(s => s.GetAllAsync()).ReturnsAsync(contacts);

        //    // Act
        //    var result = await _controller.Contacts();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(contacts, okResult.Value);
        //}

        //[Fact]
        //public async Task GetContactsByDdd_ShouldReturnOk_WithContactsByDdd()
        //{
        //    // Arrange
        //    var ddd = 11;
        //    var contacts = new List<ContactResponseDTO> { new ContactResponseDTO { Id = 1, Name = "Test", DDD = 11, Email = "test@email.com", Telefone = 55555555, DataCriacao = DateTime.Now } };
        //    _mockContactService.Setup(s => s.GetAllContactsByDDDAsync(ddd)).ReturnsAsync(contacts);

        //    // Act
        //    var result = await _controller.GetContactsByDdd(ddd);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(contacts, okResult.Value);
        //}

        //[Fact]
        //public async Task GetContactsByDdd_ShouldReturnNoContent_WhenNoContactsFound()
        //{
        //    // Arrange
        //    var ddd = 99;
        //    _mockContactService.Setup(s => s.GetAllContactsByDDDAsync(ddd)).ReturnsAsync(new List<ContactResponseDTO>());

        //    // Act
        //    var result = await _controller.GetContactsByDdd(ddd);

        //    // Assert
        //    Assert.IsType<NoContentResult>(result);
        //}

        //[Fact]
        //public async Task UpdateContacts_ShouldReturnOk_WhenUpdateIsSuccessful()
        //{
        //    // Arrange
        //    var updateDTO = new ContactUpdateRequestDTO { Id = 1, Name = "Updated Name", DDD = 11, Telefone = 999999999 };

        //    // Act
        //    var result = await _controller.UpdateContacts(updateDTO);

        //    // Assert
        //    Assert.IsType<OkResult>(result);
        //    _mockContactService.Verify(s => s.updateContactAsync(updateDTO), Times.Once);
        //}

        //[Fact]
        //public async Task UpdateContacts_ShouldReturnBadRequest_WhenModelIsInvalid()
        //{
        //    // Arrange
        //    _controller.ModelState.AddModelError("Name", "Required");
        //    var updateDTO = new ContactUpdateRequestDTO();

        //    // Act
        //    var result = await _controller.UpdateContacts(updateDTO);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task DeleteContacts_ShouldReturnOk_WhenDeleteIsSuccessful()
        //{
        //    // Arrange
        //    var contactId = 1;

        //    // Act
        //    var result = await _controller.DeleteContacts(contactId);

        //    // Assert
        //    Assert.IsType<OkResult>(result);
        //    _mockContactService.Verify(s => s.deleteContactAsync(contactId), Times.Once);
        //}
        #endregion
    }
}
