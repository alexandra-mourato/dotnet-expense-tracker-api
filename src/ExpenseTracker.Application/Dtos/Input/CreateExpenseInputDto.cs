namespace ExpenseTracker.Application.Dtos.Input;

public class CreateExpenseInputDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Guid CategoryId { get; set; }
}