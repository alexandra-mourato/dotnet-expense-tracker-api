using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ExpenseDto>>> GetAll([FromQuery] GetExpensesInputDto input)
    {
        var result = await _expenseService.GetAllAsync(input);

        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExpenseDto>> GetById(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(expense);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(CreateExpenseInputDto input)
    {
        var expense = await _expenseService.CreateAsync(input);
        
        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }
}