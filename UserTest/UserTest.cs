using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UserTest;

public class UserTest : IClassFixture<ApplicationFactory<BU2Todo.Program>>
{
  private readonly ApplicationFactory<BU2Todo.Program> _factory;

  public UserTest(ApplicationFactory<BU2Todo.Program> factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task RegisterUser_ReturnsSuccessStatusCode()
  {
    // Arrange
    // Create Client
    var client = _factory.CreateClient();

    // Fake user 
    var user = new { Email = "test@email.com", Password = "Abc123!" };

    var content = new StringContent(
      JsonSerializer.Serialize(user),
      Encoding.UTF8,
      "application/json")
    ;

    // Act
    var request = await client.PostAsync($"/register", content);

    // Assert
    request.EnsureSuccessStatusCode();

  }

  [Fact]
  public async Task LoginUser_ReturnsSuccessStatusCode()
  {
    // Arrange
    var client = _factory.CreateClient();

    // Fake user 
    var user = new { Email = "test@email.com", Password = "Abc123!" };

    var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

    // Act
    var request = await client.PostAsync("/login", content);

    // Assert 
    request.EnsureSuccessStatusCode();
  }
}