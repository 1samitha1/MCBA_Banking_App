using System.Security.Claims;
using AdminApi.Auth;
using AdminApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace AdminApi.Test.Controllers;

public class AuthControllerTest
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly IOptions<AdminAuthOptions> _adminOptions;
    private readonly AuthController _controller;
    
    public AuthControllerTest()
    {
        _mockTokenService = new Mock<ITokenService>();
        _adminOptions = Options.Create(new AdminAuthOptions
        {
            Username = "admin",
            Password = "admin"
        });
        _controller = new AuthController(_adminOptions, _mockTokenService.Object);
    }
    
    [Fact]
    public void GetTokenTest()
    {

        var request = new AuthController.LoginRequest("admin", "admin");
        _mockTokenService
            .Setup(s => s.CreateToken(It.IsAny<Claim[]>()))
            .Returns("mocked-jwt-token");

        var result = _controller.GetToken(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<AuthController.LoginResponse>(okResult.Value);

        Assert.Equal("admin", response.Username);
        Assert.Equal("mocked-jwt-token", response.AccessToken);
    }
    
    [Fact]
    public void GetTokenInvalidUserNameTest()
    {
        var request = new AuthController.LoginRequest("admin2", "admin");

        var result = _controller.GetToken(request);

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Contains("Invalid username", unauthorized.Value.ToString());
    }
}