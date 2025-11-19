namespace Core.Domain.Pagination;

public interface ICursorPaginationRequest
{
    SortDirection Direction { get; }
}
