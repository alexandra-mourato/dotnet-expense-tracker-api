using System.Net;
using System.Net.Http.Json;

namespace ExpenseTracker.Api.Tests.Controllers;

public class ExpensesControllerTests : TestBase, IClassFixture<CustomWebApplicationFactory>
{
    public ExpensesControllerTests(CustomWebApplicationFactory factory) : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task Create_Should_Return_Created_When_Request_Is_Valid()
    {
        var categoryResponse = await Client.PostAsJsonAsync(
            "/api/categories",
            new { Name = "Food" });

        var category = await categoryResponse.Content
            .ReadFromJsonAsync<CategoryResponse>();

        var request = new
        {
            Description = "Lunch",
            Amount = 12.50m,
            Date = new DateTime(2026, 5, 13),
            CategoryId = category!.Id
        };

        var response = await Client.PostAsJsonAsync("/api/expenses", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok()
    {
        var response = await Client.GetAsync("/api/expenses?pageNumber=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_Should_Return_BadRequest_When_PageNumber_Is_Invalid()
    {
        var response = await Client.GetAsync("/api/expenses?pageNumber=0&pageSize=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Category_Does_Not_Exist()
    {
        var request = new
        {
            Description = "Lunch",
            Amount = 12.50m,
            Date = new DateTime(2026, 5, 13),
            CategoryId = Guid.NewGuid()
        };

        var response = await Client.PostAsJsonAsync("/api/expenses", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private sealed class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}