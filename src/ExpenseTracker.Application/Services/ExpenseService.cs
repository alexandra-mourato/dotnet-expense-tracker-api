using ExpenseTracker.Application.Common.Constants;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Interfaces.Persistence;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ExpenseService(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResultDto<ExpenseDto>> GetAllAsync(GetExpensesInputDto input)
    {
        var totalItems = await _expenseRepository.CountAsync(
            input.Month,
            input.Year);

        var expenses = await _expenseRepository.GetPagedAsync(
            input.Month,
            input.Year,
            input.PageNumber,
            input.PageSize);

        return new PagedResultDto<ExpenseDto>
        {
            Items = expenses.Select(MapToDto),
            PageNumber = input.PageNumber,
            PageSize = input.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)input.PageSize)
        };
    }

    public async Task<ExpenseDto?> GetByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        return expense is null ? null : MapToDto(expense);
    }

    public async Task<ExpenseDto?> CreateAsync(CreateExpenseInputDto input)
    {
        var categoryExists = await _categoryRepository.ExistsAsync(input.CategoryId);

        if (!categoryExists)
        {
            throw new BadRequestException(ErrorMessages.CategoryDoesNotExist);
        }

        var expense = new Expense
        {
            Description = input.Description,
            Amount = input.Amount,
            Date = input.Date,
            CategoryId = input.CategoryId
        };

        await _expenseRepository.AddAsync(expense);

        var createdExpense = await _expenseRepository.GetByIdAsync(expense.Id);

        return MapToDto(createdExpense!);
    }

    private static ExpenseDto MapToDto(Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category.Name
        };
    }
}