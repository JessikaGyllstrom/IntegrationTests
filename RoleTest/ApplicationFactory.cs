using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RoleTest;
public class ApplicationFactory<T> : WebApplicationFactory<T>
    where T : class
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      // Create a mock database using sqlite
      services.AddDbContext<BU2Todo.ApplicationContext>(options =>
      {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        options.UseSqlite($"Data source={Path.Join(path, "testdb.db")}");
      });
      services
        .AddAuthentication("TestScheme")
        .AddScheme<AuthenticationSchemeOptions, MyAuthHandler>("TestScheme", options => { }
      );

      // Reference to database
      var context = CreateApplicationContext(services);

      // Delete data 
      context.Database.EnsureDeleted();
      // Create new data
      context.Database.EnsureCreated();

      // Create user and save to database
      var user = new BU2Todo.User();
      context.Users.Add(user);
      context.SaveChanges();

    });
  }

  static BU2Todo.ApplicationContext CreateApplicationContext(IServiceCollection services)
  {
    var provider = services.BuildServiceProvider();
    var scope = provider.CreateScope();
    return scope.ServiceProvider.GetRequiredService<BU2Todo.ApplicationContext>();
  }
}

public class MyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  public MyAuthHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder
  )
      : base(options, logger, encoder) { }

  // Generate fake token 
  protected override Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, "my-test-id"),
      new Claim(ClaimTypes.Name, "test-name"),
      new Claim(ClaimTypes.Role, "admin")
    };

    var identity = new ClaimsIdentity(claims, "Test");
    var principal = new ClaimsPrincipal(identity);
    var ticket = new AuthenticationTicket(principal, "TestScheme");

    var result = AuthenticateResult.Success(ticket);
    return Task.FromResult(result);

  }
}