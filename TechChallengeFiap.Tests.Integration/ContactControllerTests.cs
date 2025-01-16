using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using TechChallengeFiap.Infrastructure.DTOs;
using TechChallengeFiap.Tests.Integration;

public class ContactControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ContactControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithContacts()
    {
        // Act
        var response = await _client.GetAsync("/Contact/GetAll");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Assert.NotNull(json);

        var contacts = JsonConvert.DeserializeObject<List<ContactResponseDTO>>(json);

        Assert.True(contacts?.Count > 0);
    }

    [Fact]
    public async Task AddContact_ShouldReturnCreated()
    {
        // Arrange
        var newContact = new
        {
            Name = "Gustavo Macedo",
            Email = "gustavo.macedo@example.com",
            DDD = 11,
            Telefone = 987654321
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Contact/AddContact", newContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
