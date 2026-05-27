using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Dtos.Output;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterInputDto input)
    {
        await _authService.RegisterAsync(input);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginOutputDto>> Login([FromBody] LoginInputDto input)
    {
        var result = await _authService.LoginAsync(input);

        return Ok(result);
    }
}