using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ExpenseTracker.Api.Tests;

public abstract class TestBase
{
    protected readonly HttpClient Client;

    protected TestBase(HttpClient client)
    {
        Client = client;
        
        AuthenticateAsync()
            .GetAwaiter()
            .GetResult();
    }

    protected async Task AuthenticateAsync()
    {
        var registerRequest = new
        {
            Email = $"{Guid.NewGuid()}@test.com",
            Password = "Password123"
        };

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            registerRequest);

        var loginResponse = await Client.PostAsJsonAsync(
            "/api/auth/login",
            registerRequest);

        var loginResult = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token);
    }

    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}