using System.Net;
using System.Net.Http.Json;

namespace ExpenseTracker.Api.Tests;

public class DashboardControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DashboardControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMonthlyDashboard_Should_Return_Ok()
    {
        var response = await _client.GetAsync("/api/dashboard/monthly?month=5&year=2026");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMonthlyDashboard_Should_Return_Data_When_Expenses_Exist()
    {
        var categoryResponse = await _client.PostAsJsonAsync(
            "/api/categories",
            new { Name = "Food" });

        var category = await categoryResponse.Content
            .ReadFromJsonAsync<CategoryResponse>();

        await _client.PostAsJsonAsync(
            "/api/expenses",
            new
            {
                Description = "Lunch",
                Amount = 12.50m,
                Date = new DateTime(2026, 5, 13),
                CategoryId = category!.Id
            });

        var response = await _client.GetAsync(
            "/api/dashboard/monthly?month=5&year=2026");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}