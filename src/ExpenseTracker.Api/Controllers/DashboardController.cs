using ExpenseTracker.Application.Dtos.Output;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<DashboardOutputDto>> GetMonthlyDashboard(
        [FromQuery] int month,
        [FromQuery] int year)
    {
        var dashboard = await _dashboardService.GetMonthlyDashboardAsync(
            month,
            year);

        return Ok(dashboard);
    }
}