using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(Guid id);

    Task AddAsync(Category category);
    
    Task<bool> ExistsAsync(Guid id);
}