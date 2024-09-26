using Auth.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Auth.Tests.ApiEndpoints;

[TestFixture]
public class ApiEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {});
        _httpClient = _factory.CreateClient();
    }

    [Test]
    public async Task ServiceAlive_Should_Return_ServiceAliveDto()
    {
        // Act
        var response = await _httpClient.GetAsync("/api");
        response.EnsureSuccessStatusCode();

        var serviceAlive = await response.Content.ReadFromJsonAsync<ServiceAlive>();

        // Assert
        Assert.That(serviceAlive.Version, Is.EqualTo(Assembly.GetEntryAssembly().GetName().Version.ToString()));
    }

    [Test]
    public async Task Login_Should_Call_Manager_And_Return_UserDto()
    {
        // Arrange
        var loginDto = new Login { Email = "test", Password = "pw" };
        var loginJson = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/login", loginJson);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UserDto>();

        // Assert
        Assert.That(result.Email, Is.EqualTo(loginDto.Email));
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
