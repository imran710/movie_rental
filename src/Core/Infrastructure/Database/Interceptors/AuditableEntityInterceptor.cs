using Core.Domain.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Infrastructure.Database.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? dbContext)
    {
        if (dbContext is null) return;

        foreach (var entry in dbContext.ChangeTracker.Entries<IDeletableEntity>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.DeletionInfo.MarkAsDeleted(0);
            }
        }

        foreach (var entry in dbContext.ChangeTracker.Entries<IUpdatableEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateInfo.MarkAsUpdated(0);
            }
        }
    }
}
