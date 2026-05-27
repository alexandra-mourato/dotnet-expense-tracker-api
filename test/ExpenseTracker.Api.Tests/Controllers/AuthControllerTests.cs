using System.Net;
using System.Net.Http.Json;

namespace ExpenseTracker.Api.Tests.Controllers;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Return_Ok_When_Request_Is_Valid()
    {
        var request = new
        {
            Email = "test@test.com",
            Password = "Password123"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Register_Should_Return_BadRequest_When_Email_Already_Exists()
    {
        var request = new
        {
            Email = "duplicate@test.com",
            Password = "Password123"
        };

        await _client.PostAsJsonAsync("/api/auth/register", request);

        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_Should_Return_Ok_When_Credentials_Are_Valid()
    {
        var request = new
        {
            Email = "login@test.com",
            Password = "Password123"
        };

        await _client.PostAsJsonAsync("/api/auth/register", request);

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_Credentials_Are_Invalid()
    {
        var request = new
        {
            Email = "missing@test.com",
            Password = "WrongPassword"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}