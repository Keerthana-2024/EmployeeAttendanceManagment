using EmployeeAttendanceManagment.Controllers;
using EmployeeAttendanceManagment.Model;
using EmployeeAttendanceManagment.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class TokenControllerTests
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly TokenController _controller;

    public TokenControllerTests()
    {
        _mockTokenService = new Mock<ITokenService>();
        _controller = new TokenController(_mockTokenService.Object);
    }

    [Fact]
    public void GenerateToken_ReturnsOkResult_WithToken()
    {
        // Arrange
        var username = "testuser";
        var token = "generatedToken";
        var model = new TokenRequest { Username = username };

        // Setup mock to return the token
        _mockTokenService.Setup(service => service.GenerateToken(username)).Returns(token);

        // Act
        var result = _controller.GenerateToken(model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value as dynamic;

        Assert.NotNull(value); // Ensure the response object is not null

        // Check if the response contains the 'Token' property
        var tokenProperty = value.GetType().GetProperty("Token");
        Assert.NotNull(tokenProperty); // Ensure 'Token' property exists

        var generatedToken = tokenProperty.GetValue(value)?.ToString();
        Assert.NotNull(generatedToken); // Ensure the token is not null
        Assert.Equal(token, generatedToken); // Verify the token value
    }

    [Fact]
    public void GenerateToken_ReturnsBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var model = new TokenRequest { Username = string.Empty };

        // Simulate invalid model state
        _controller.ModelState.AddModelError("Username", "Required");

        // Act
        var result = _controller.GenerateToken(model);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var modelState = badRequestResult.Value as SerializableError;
        Assert.NotNull(modelState);
        Assert.True(modelState.ContainsKey("Username"));
    }
}
