using System.Net;
using System.Net.Http.Json;

namespace ExpenseTracker.Api.Tests;

public class CategoriesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Should_Return_Created_When_Request_Is_Valid()
    {
        var request = new { Name = "Food" };

        var response = await _client.PostAsJsonAsync("/api/categories", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok()
    {
        var response = await _client.GetAsync("/api/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Name_Is_Empty()
    {
        var request = new { Name = "" };

        var response = await _client.PostAsJsonAsync("/api/categories", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}