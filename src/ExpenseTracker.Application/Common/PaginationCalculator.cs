namespace ExpenseTracker.Application.Common;

public static class PaginationCalculator
{
    public static int CalculateSkip(int pageNumber, int pageSize)
    {
        if (pageNumber <= 1 || pageSize <= 0)
        {
            return 0;
        }
        
        return (pageNumber - 1) * pageSize;
    }
    
    public static int CalculateTotalPages(int totalItems, int pageSize)
    {
        if (pageSize <= 0)
        {
            return 0;
        }
        
        return (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}