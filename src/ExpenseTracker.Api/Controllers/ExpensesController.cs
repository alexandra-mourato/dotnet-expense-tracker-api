using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseTrackerDbContext _context;
    private readonly IValidator<CreateExpenseDto> _validator;

    public ExpensesController(
        ExpenseTrackerDbContext context,
        IValidator<CreateExpenseDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll(
        [FromQuery] int? month,
        [FromQuery] int? year)
    {
        var query = _context.Expenses
            .AsNoTracking()
            .AsQueryable();

        if (month.HasValue)
        {
            query = query.Where(e => e.Date.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(e => e.Date.Year == year.Value);
        }

        var expenses = await query
            .OrderByDescending(e => e.Date)
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                Description = e.Description,
                Amount = e.Amount,
                Date = e.Date,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name
            })
            .ToListAsync();

        return Ok(expenses);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExpenseDto>> GetById(Guid id)
    {
        var expense = await _context.Expenses
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                Description = e.Description,
                Amount = e.Amount,
                Date = e.Date,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name
            })
            .FirstOrDefaultAsync();

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(expense);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(CreateExpenseDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
        {
            return BadRequest("Category does not exist.");
        }

        var expense = new Expense
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            CategoryId = dto.CategoryId
        };

        _context.Expenses.Add(expense);

        await _context.SaveChangesAsync();

        var category = await _context.Categories
            .FirstAsync(c => c.Id == dto.CategoryId);

        var result = new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.CategoryId,
            CategoryName = category.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, result);
    }
}