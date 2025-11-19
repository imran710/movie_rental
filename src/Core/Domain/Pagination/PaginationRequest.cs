namespace Core.Domain.Pagination;

public class PaginationRequest : IPaginationRequest
{
    private const int MaxPageSize = 250;

    private int _pageSize = 10;
    private int? _pageNumber;

    public int? PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }

    public static PaginationRequest CreateWithOffset(int pageNumber, int pageSize) => new()
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
    };
}
