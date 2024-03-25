
namespace RoleTest;

public class RoleTest : IClassFixture<ApplicationFactory<BU2Todo.Program>>
{
  private readonly ApplicationFactory<BU2Todo.Program> _factory;

  public RoleTest(ApplicationFactory<BU2Todo.Program> factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task CreateUserRole_ReturnsSuccessStatusCode()
  {
    // Arrange
    // Create Client
    var client = _factory.CreateClient();

    var testRole = "test-role";

    // Act
    // Send request 
    var request = await client.PostAsync($"/api/role/{testRole}", null);

    // Assert 
    // If response is successfull
    request.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task AddUserToRole_ReturnsSuccessStatusCode()
  {
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var request = await client.PostAsync("/api/role-add/test-role", null);

    // Assert 
    request.EnsureSuccessStatusCode();
  }
}