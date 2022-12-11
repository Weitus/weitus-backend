using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using weitus_backend.Controllers;
using weitus_backend.Data;
using weitus_backend.Data.Dto;
using weitus_backend.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace weitus_backend_tests;

public class ChatControllerTests
{
    internal static ChatController CreateMockChatController()
    {
        var options = new DbContextOptionsBuilder<WeitusDbContext>()
            .UseInMemoryDatabase(databaseName: "WeitusDatabase")
            .Options;
        var context = new WeitusDbContext(options);

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

        var repo = new WeitusRepository(context, userManager);

        var chatController = new ChatController(new NullLogger<ChatController>(), repo, userManager);
        return chatController;
    }

    [Fact]
    public async Task TestGetMessages()
    {
        var controller = CreateMockChatController();
        var auth = AuthControllerTests.CreateAuthController();

        var registerResult = await auth.Register(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@example.org"
        });

        Assert.IsType<OkResult>(registerResult);

        var loginResult = await auth.Login(new LoginUser()
        {
            UserName = "test",
            Password = "test"
        });

        Assert.IsType<OkObjectResult>(loginResult);

        var loginResultValue = loginResult as OkObjectResult;

        Assert.IsType<AuthenticationResponse>(loginResultValue.Value);

        var loginResultValueValue = loginResultValue.Value as AuthenticationResponse;

        var token = loginResultValueValue.Token;

        JwtSecurityToken jwtToken = new JwtSecurityToken(token);

        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims))
        };

        var result = await controller.GetMessages();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task TestSendMessage() {
        var controller = CreateMockChatController();
        var auth = AuthControllerTests.CreateAuthController();

        var registerResult = await auth.Register(new RegisterUser()
        {
            UserName = "test",
            Password = "test",
            Email = "test@example.org"
        });

        Assert.IsType<OkResult>(registerResult);

        var loginResult = await auth.Login(new LoginUser()
        {
            UserName = "test",
            Password = "test"
        });

        Assert.IsType<OkObjectResult>(loginResult);

        var loginResultValue = loginResult as OkObjectResult;

        Assert.IsType<AuthenticationResponse>(loginResultValue.Value);

        var loginResultValueValue = loginResultValue.Value as AuthenticationResponse;

        var token = loginResultValueValue.Token;

        JwtSecurityToken jwtToken = new JwtSecurityToken(token);

        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims))
        };

        var sendResult = await controller.SendMessage(new SendChatMessage()
        {
            Message = "test"
        });

        Assert.IsType<OkObjectResult>(sendResult);

        var sendResult2 = await controller.SendMessage(new SendChatMessage()
        {
            Message = "test2"
        });

        Assert.IsType<OkObjectResult>(sendResult2);

        var result = await controller.GetMessages();

        Assert.IsType<OkObjectResult>(result);

        var resultValue = result as OkObjectResult;

        Assert.IsType<IEnumerable<ChatMessage>>(resultValue.Value);

        var resultValueValue = resultValue.Value as IEnumerable<ChatMessage>;

        Assert.Equal(2, resultValueValue.Count());

        Assert.Equal("test", resultValueValue.First().Message);

        Assert.Equal("test2", resultValueValue.Last().Message);
    }
}
