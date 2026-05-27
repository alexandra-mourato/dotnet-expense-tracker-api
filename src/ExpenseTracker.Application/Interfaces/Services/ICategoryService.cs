using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(Guid id);

    Task<CategoryDto> CreateAsync(CreateCategoryInputDto input);
}