using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Interfaces.Persistence;

public interface IExpenseTrackerDbContext
{
    DbSet<Expense> Expenses { get; }
    DbSet<Category> Categories { get; }
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}