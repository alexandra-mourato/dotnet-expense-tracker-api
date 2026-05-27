using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IExpenseService
{
    Task<PagedResultDto<ExpenseDto>> GetAllAsync(
        GetExpensesInputDto input);

    Task<ExpenseDto?> GetByIdAsync(Guid id);

    Task<ExpenseDto?> CreateAsync(CreateExpenseInputDto input);
}