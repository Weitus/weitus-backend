#nullable disable

using Xunit;
using weitus_backend.Data;
using weitus_backend.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using weitus_backend.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weitus_backend.Data.Dto;

namespace weitus_backend_tests;

public class AuthControllerTests
{
    internal static AuthController CreateAuthController(WeitusDbContext context = null)
    {
        if (context == null)
        {
            context = new WeitusDbContext(new DbContextOptionsBuilder<WeitusDbContext>()
                .UseInMemoryDatabase("AuthControllerTests")
                .Options);
        }

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var config = new MockConfiguration();
        var logger = new NullLogger<AuthController>();
        var repo = new WeitusRepository(context, config);
        var jwtService = new JwtService(config);
        var userManager = new UserManager(repo, jwtService, config);

        return new AuthController(logger, userManager);
    }

    [Fact]
    public async Task TestRegister()
    {
        var controller = CreateAuthController();

        var result = await controller.Register(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@example.org"
        });

        Assert.IsType<OkResult>(result);

        var loginResult = await controller.Login(new LoginUser()
        {
            UserName = "test",
            Password = "test"
        });

        Assert.IsType<OkObjectResult>(loginResult);

        var loginResultValue = loginResult as OkObjectResult;

        Assert.IsType<AuthenticationResponse>(loginResultValue.Value);

        var loginResultValueValue = loginResultValue.Value as AuthenticationResponse;

        Assert.NotNull(loginResultValueValue.Token);
    }

    [Fact]
    public async Task TestRegisterDouble()
    {
        var controller = CreateAuthController();

        var result = await controller.Register(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@example.org"
        });

        Assert.IsType<OkResult>(result);

        var result2 = await controller.Register(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@example.org"
        });

        Assert.IsType<BadRequestObjectResult>(result2);
    }

    [Fact]
    public async Task TestLoginUnknownAccount()
    {
        var controller = CreateAuthController();

        var result = await controller.Login(new LoginUser()
        {
            UserName = "test",
            Password = "test"
        });

        Assert.IsType<BadRequestObjectResult>(result);

        var resultValue = result as BadRequestObjectResult;

        Assert.IsType<ErrorResponse>(resultValue.Value);

        var resultValueValue = resultValue.Value as ErrorResponse;

        Assert.Equal("Bad username/password combination", resultValueValue.Message);
    }

    [Fact]
    public async Task TestLoginBadCredentials()
    {
        var controller = CreateAuthController();

        var result = await controller.Register(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@example.org"
        });

        Assert.IsType<OkResult>(result);

        var badLoginResult = await controller.Login(new LoginUser()
        {
            UserName = "test",
            Password = "test2"
        });

        Assert.IsType<BadRequestObjectResult>(badLoginResult);

        var badLoginResultValue = badLoginResult as BadRequestObjectResult;

        Assert.IsType<ErrorResponse>(badLoginResultValue.Value);

        var badLoginResultValueValue = badLoginResultValue.Value as ErrorResponse;

        Assert.Equal("Bad username/password combination", badLoginResultValueValue.Message);

        var loginResult = await controller.Login(new LoginUser()
        {
            UserName = "test",
            Password = "test"
        });

        Assert.IsType<OkObjectResult>(loginResult);

        var loginResultValue = loginResult as OkObjectResult;

        Assert.IsType<AuthenticationResponse>(loginResultValue.Value);

        var loginResultValueValue = loginResultValue.Value as AuthenticationResponse;

        Assert.NotNull(loginResultValueValue.Token);
    }
}
