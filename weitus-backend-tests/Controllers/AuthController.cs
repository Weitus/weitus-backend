#nullable disable

using Xunit;
using weitus_backend.Data;
using weitus_backend.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Identity;
using weitus_backend.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using weitus_backend.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weitus_backend.Data.Dto;

namespace weitus_backend_tests;

class MockConfiguration : IConfiguration
{
    public string this[string key]
    {
        get
        {
            switch (key)
            {
                case "Jwt:Issuer":
                    return "http://localhost:5000";
                case "Jwt:Audience":
                    return "http://localhost:5000";
                case "Jwt:Key":
                    return "0123456789ABCDEF0123456789ABCDEF";
                default:
                    return "";
            }
        }
        set => throw new System.NotImplementedException();
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new System.NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new System.NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        throw new System.NotImplementedException();
    }
}

class RelaxedPasswordValidator : PasswordValidator<WeitusUser>
{
    public override Task<IdentityResult> ValidateAsync(UserManager<WeitusUser> manager, WeitusUser user, string password)
    {
        return Task.FromResult(IdentityResult.Success); // This could be improved to check 
                                                        // the password *exactly* like the
                                                        // requirements in the app, but they are
                                                        // so low as of right now this is enough.
    }
}

public class AuthControllerTests
{
    internal static AuthController CreateAuthController(WeitusDbContext context = null)
    {
        if (context == null)
        {
            context = new WeitusDbContext(new DbContextOptionsBuilder<WeitusDbContext>()
                .UseInMemoryDatabase("WeitusDatabase")
                .Options);
        }

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var logger = new NullLogger<AuthController>();
        var userManager = new UserManager<WeitusUser>(
            new UserStore<WeitusUser>(context),
            null,
            new PasswordHasher<WeitusUser>(),
            new UserValidator<WeitusUser>[1] {
                new UserValidator<WeitusUser>()
            },
            new IPasswordValidator<WeitusUser>[1] { new RelaxedPasswordValidator() },
            null,
            null,
            null,
            new NullLogger<UserManager<WeitusUser>>());

        var jwtService = new JwtService(new MockConfiguration());

        return new AuthController(logger, userManager, jwtService);
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
