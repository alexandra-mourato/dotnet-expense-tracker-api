namespace ExpenseTracker.Application.Dtos;

public class CategoryTotalDto
{
    public string Category { get; set; } = string.Empty;

    public decimal Total { get; set; }
}