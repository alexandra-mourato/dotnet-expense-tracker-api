using ExpenseTracker.Api.Filters;
using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;
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
    [ServiceFilter(typeof(ValidationFilter<GetExpensesInputDto>))]
    public async Task<ActionResult<PagedResultDto<ExpenseDto>>> GetAll(
        [FromQuery] GetExpensesInputDto inputDto)
    {
        var query = _context.Expenses
            .AsNoTracking()
            .AsQueryable();

        if (inputDto.Month.HasValue)
        {
            query = query.Where(e => e.Date.Month == inputDto.Month.Value);
        }

        if (inputDto.Year.HasValue)
        {
            query = query.Where(e => e.Date.Year == inputDto.Year.Value);
        }

        var totalItems = await query.CountAsync();

        var expenses = await query
            .OrderByDescending(e => e.Date)
            .Skip((inputDto.PageNumber - 1) * inputDto.PageSize)
            .Take(inputDto.PageSize)
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

        var result = new PagedResultDto<ExpenseDto>
        {
            Items = expenses,
            PageNumber = inputDto.PageNumber,
            PageSize = inputDto.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)inputDto.PageSize)
        };

        return Ok(result);
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
    [ServiceFilter(typeof(ValidationFilter<CreateExpenseDto>))]
    public async Task<ActionResult<ExpenseDto>> Create(CreateExpenseDto dto)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

        if (category is null)
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