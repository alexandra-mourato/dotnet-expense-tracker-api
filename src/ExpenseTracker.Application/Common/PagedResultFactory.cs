using ExpenseTracker.Application.Dtos;

namespace ExpenseTracker.Application.Common;

public static class PagedResultFactory
{
    public static PagedResultDto<T> Create<T>(
        IReadOnlyCollection<T> items,
        int totalItems,
        int pageNumber,
        int pageSize)
    {
        return new PagedResultDto<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = PaginationCalculator.CalculateTotalPages(totalItems, pageSize)
        };
    }
}