using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);
}