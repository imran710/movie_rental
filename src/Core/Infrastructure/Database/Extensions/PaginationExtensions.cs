using Core.Domain.Entity;
using Core.Domain.Pagination;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Database.Extensions;

public static class PaginationExtensions
{
    public static async Task<OffsetPaginatedResponse<T>> ApplyPagination<T>(this IQueryable<T> query, IPaginationRequest request, CancellationToken cancellationToken = default) where T : class
    {
        // Get total count first
        var totalItems = await query.CountAsync(cancellationToken);

        // If no items, return empty response
        if (totalItems == 0)
        {
            return new OffsetPaginatedResponse<T>();
        }

        // Calculate skip and take values
        var skip = ((request.PageNumber ?? 1) - 1) * request.PageSize;

        // Get paginated data
        var data = await query
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Create pagination request object to match constructor requirements
        var paginationRequest = new PaginationRequest
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return new OffsetPaginatedResponse<T>(data, paginationRequest, totalItems);
    }
}
