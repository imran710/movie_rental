using Core.Domain.Entity;

namespace Core.Domain.Pagination;

public class CursorPaginatedResponse<T> where T : class
{
    private static readonly List<T> EmptyList = [];

    public IList<T> Data { get; private set; } = EmptyList;
    public long? NextCursor { get; private set; }
    public long? PreviousCursor { get; private set; }
    public bool HasNextPage { get; private set; }
    public bool HasPreviousPage { get; private set; }

    public CursorPaginatedResponse() { }

    private CursorPaginatedResponse(IList<T> data, long? nextCursor, long? previousCursor, bool hasNext, bool hasPrevious)
    {
        Data = data ?? EmptyList;
        NextCursor = nextCursor;
        PreviousCursor = previousCursor;
        HasNextPage = hasNext;
        HasPreviousPage = hasPrevious;
    }

    public static CursorPaginatedResponse<T> Create(IList<T> items, long? nextCursor, long? previousCursor, bool hasNext, bool hasPrevious)
    {
        return new CursorPaginatedResponse<T>(items, nextCursor, previousCursor, hasNext, hasPrevious);
    }

    public static CursorPaginatedResponse<TValue> CreateFromRequest<TValue>(
        IList<TValue> items,
        ICursorPaginationRequest request,
        bool hasNext,
        bool hasPrevious) where TValue : class, IEntity
    {
        if (items == null || items.Count == 0)
            return new CursorPaginatedResponse<TValue>();

        long? nextCursor = request.Direction == SortDirection.Forward ? items[^1].Id : null;
        long? previousCursor = request.Direction == SortDirection.Backward ? items[0].Id : null;

        return new CursorPaginatedResponse<TValue>
        {
            Data = items,
            NextCursor = nextCursor,
            PreviousCursor = previousCursor,
            HasNextPage = hasNext,
            HasPreviousPage = hasPrevious
        };
    }
}
