namespace ExpenseTracker.Application.Dtos;

public class TotalByCategoryDto
{
    public string Category { get; set; } = string.Empty;

    public decimal Total { get; set; }
}