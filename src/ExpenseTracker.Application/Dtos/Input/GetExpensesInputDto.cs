namespace ExpenseTracker.Application.Dtos.Input;

public class GetExpensesInputDto : PaginationDto
{
    public int? Month { get; set; }

    public int? Year { get; set; }
}