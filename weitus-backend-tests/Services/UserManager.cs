using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using weitus_backend.Data;
using weitus_backend.Data.Dto;
using weitus_backend.Services;
using Xunit;

namespace weitus_backend_tests;

public class UserManagerTests
{
    public static UserManager CreateUserManager()
    {
        var context = new WeitusDbContext(new DbContextOptionsBuilder<WeitusDbContext>()
            .UseInMemoryDatabase("UserManagerTests")
            .Options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var repo = new WeitusRepository(context);
        var jwtService = new JwtService(new MockConfiguration());

        return new UserManager(repo, jwtService);
    }

    [Fact]
    public async Task TestRegister()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);
    }

    [Fact]
    public async Task TestRegisterDoubleUsername()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var registerResult2 = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "different",
            Email = "different@test.com"
        });

        Assert.False(registerResult2.Success);

        Assert.Equal(1, registerResult2.Errors.Length);
        Assert.True(registerResult2.Errors.Any(e => e == "Username is already taken"));
    }

    [Fact]
    public async Task TestRegisterDoubleEmail()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var registerResult2 = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "different",
            Password = "different",
            Email = "test@test.com"
        });

        Assert.False(registerResult2.Success);

        Assert.Equal(1, registerResult2.Errors.Length);
        Assert.True(registerResult2.Errors.Any(e => e == "Email is already in use"));
    }

    [Fact]
    public async Task TestRegisterDoublePassword()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var registerResult2 = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "different",
            Password = "test",
            Email = "different@test.com"
        });

        Assert.True(registerResult2.Success);
    }

    [Fact]
    public async Task TestGetUserById()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var user = await userManager.GetUserAsync(registerResult.Value.UserId);

        Assert.NotNull(user);

        Assert.Equal("test", user.UserName);

        Assert.Equal("test@test.com", user.Email);

        Assert.Equal(registerResult.Value.UserId, user.UserId);
    }

    [Fact]
    public async Task TestGetUserByUsername()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var user = await userManager.GetUserAsync(registerResult.Value.UserName);

        Assert.NotNull(user);

        Assert.Equal("test", user.UserName);

        Assert.Equal("test@test.com", user.Email);

        Assert.Equal(registerResult.Value.UserId, user.UserId);
    }

    [Fact]
    public async Task TestGetUserByEmail()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var user = await userManager.GetUserByEmailAsync(registerResult.Value.Email);

        Assert.NotNull(user);

        Assert.Equal("test", user.UserName);

        Assert.Equal("test@test.com", user.Email);

        Assert.Equal(registerResult.Value.UserId, user.UserId);
    }

    [Fact]
    public async Task TestGetUserByClaims()
    {
        var userManager = CreateUserManager();

        var registerResult = await userManager.RegisterUserAsync(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@test.com"
        });

        Assert.True(registerResult.Success);

        var user = await userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, registerResult.Value.UserId.ToString())
        })));

        Assert.NotNull(user);

        Assert.Equal("test", user.UserName);

        Assert.Equal("test@test.com", user.Email);

        Assert.Equal(registerResult.Value.UserId, user.UserId);
    }
}