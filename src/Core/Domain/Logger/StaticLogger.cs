using Microsoft.Extensions.Logging;

namespace Core.Domain.Logger;

public partial class StaticLogger
{
    public static ILogger<StaticLogger> Logger = default!;
}
