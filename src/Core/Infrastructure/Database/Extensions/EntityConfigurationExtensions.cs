using Core.Domain.Audit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UnitsNet;

namespace Core.Infrastructure.Database.Extensions;

public static class EntityConfigurationExtensions
{
    public static void AddCreationInfoConfig(this ComplexPropertyBuilder<CreationInfo> options)
    {
        options.Property(p => p.CreatedAt).IsRequired().HasColumnName("CreatedAt");
        options.Property(p => p.CreatedBy).IsRequired().HasColumnName("CreatedBy");
    }

    public static void AddUpdatableConfig(this ComplexPropertyBuilder<UpdateInfo> options)
    {
        options.Property(p => p.UpdatedAt).IsRequired(false).HasColumnName("UpdatedAt");
        options.Property(p => p.UpdatedBy).IsRequired(false).HasColumnName("UpdatedBy");
    }

    public static void AddCreationTimeConfig(this ComplexPropertyBuilder<CreationTime> options)
    {
        options.Property(p => p.CreatedAt).IsRequired().HasColumnName("CreatedAt");
    }

    public static void AddDeletableConfig(this ComplexPropertyBuilder<DeletionInfo> options)
    {
        options.Property(p => p.IsDeleted).HasColumnName("IsDeleted");
        options.Property(p => p.DeletedBy).IsRequired(false).HasColumnName("DeletedBy");
        options.Property(p => p.DeletedAt).IsRequired(false).HasColumnName("DeletedAt");
    }

    public static void AddEnergyConfig(this ComplexPropertyBuilder<Energy> options)
    {
        options.Property(p => p.Unit);
        options.Property(p => p.Value);
        options.Property(p => p.Calories);
    }

    public static void AddMassConfig(this ComplexPropertyBuilder<Mass> options)
    {
        options.Property(p => p.Unit);
        options.Property(p => p.Value);
        options.Property(p => p.Grams);
    }
}
