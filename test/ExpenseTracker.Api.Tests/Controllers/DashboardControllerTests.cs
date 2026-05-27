using System.Net;
using System.Net.Http.Json;

namespace ExpenseTracker.Api.Tests.Controllers;

public class DashboardControllerTests : TestBase, IClassFixture<CustomWebApplicationFactory>
{
    public DashboardControllerTests(CustomWebApplicationFactory factory) : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task GetMonthlyDashboard_Should_Return_Ok()
    {
        var response = await Client.GetAsync("/api/dashboard/monthly?month=5&year=2026");

        // Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            body);
    }

    [Fact]
    public async Task GetMonthlyDashboard_Should_Return_Data_When_Expenses_Exist()
    {
        var categoryResponse = await Client.PostAsJsonAsync(
            "/api/categories",
            new { Name = "Food" });

        var category = await categoryResponse.Content
            .ReadFromJsonAsync<CategoryResponse>();

        await Client.PostAsJsonAsync(
            "/api/expenses",
            new
            {
                Description = "Lunch",
                Amount = 12.50m,
                Date = new DateTime(2026, 5, 13),
                CategoryId = category!.Id
            });

        var response = await Client.GetAsync(
            "/api/dashboard/monthly?month=5&year=2026");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}