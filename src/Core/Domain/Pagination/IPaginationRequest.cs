namespace Core.Domain.Pagination;

public interface IPaginationRequest
{
    public int? PageNumber { get; }
    public int PageSize { get; }
}
