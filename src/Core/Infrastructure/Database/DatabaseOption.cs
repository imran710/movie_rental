using Core.Infrastructure.Common.Enums;

namespace Core.Infrastructure.Database;

public class DatabaseOption
{
    public const string SectionName = "Database";

    public required DatabaseType DatabaseType { get; init; }
    public required string ConnectionString { get; init; }
    public required bool AutoMigrate { get; init; }
    public required bool Seed { get; init; }
}
