namespace Core.Domain.Pagination;

public class OffsetPaginatedResponse<T> where T : class
{
    private static readonly List<T> EmptyList = [];

    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }
    public int CurrentPage { get; private set; }
    public bool HasNextPage { get; private set; }
    public bool HasPreviousPage { get; private set; }
    public IList<T> Data { get; private set; } = EmptyList;

    public OffsetPaginatedResponse() { }

    public OffsetPaginatedResponse(IList<T> data, IPaginationRequest pagination, int totalItems)
    {
        Data = data ?? EmptyList;
        TotalItems = totalItems;
        CurrentPage = pagination.PageNumber ?? 0;
        TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);
        HasNextPage = CurrentPage < TotalPages;
        HasPreviousPage = CurrentPage > 1;
    }

    public OffsetPaginatedResponse(IList<T> data)
    {
        Data = data ?? EmptyList;
        TotalItems = data?.Count ?? 0;
        TotalPages = TotalItems > 0 ? 1 : 0;
        CurrentPage = TotalItems > 0 ? 1 : 0;
        HasNextPage = false;
        HasPreviousPage = false;
    }
}
