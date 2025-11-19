using Core.Domain.Entity;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Database.Extensions;

public static class QueryFilterExtensions
{
    public static void AddDeletionQueryFilter<T>(this EntityTypeBuilder<T> builder) where T : class, IDeletableEntity
    {
        builder.HasQueryFilter(x => !x.DeletionInfo.IsDeleted);
    }
}
