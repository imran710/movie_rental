using Core.Domain.Audit;
using Core.Domain.Entity;

namespace Core.Features.Users.Entity;
public class AchievementTier : IEntity, IDeletableEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinPoints { get; set; }
    public int MaxPoints { get; set; }
    public required CreationInfo CreationInfo { get; init; }
    public DeletionInfo DeletionInfo { get; } = DeletionInfo.NoDeleted();
    public bool IsWithinRange(int points)
    {
        return points >= MinPoints && points <= MaxPoints;
    }
    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }
    public static Result<AchievementTier> Create(User user, Value<string> name,int minpoint,int maxpoint)
    {
        var errors = new ValidationErrors();

        if (user is null)
            errors.Add(name, ValidationError.Create("User (creator) is required", "Error.AchievementTier.CreatorRequired"));
        if (minpoint <= 0 )
            errors.Add(name, ValidationError.Create("MinPoints is required", "Error.AchievementTier.MinPointsNonNegative"));

        if (maxpoint <= minpoint)
            errors.Add(name, ValidationError.Create("MaxPoints must be greater than MinPoints", "Error.AchievementTier.MaxPointsInvalid"));

        if (errors.HasErrors)
            return Error.Validation(errors);

        var tire = new AchievementTier
        {
            Name = name.Data!,
            MinPoints = minpoint,
            MaxPoints = maxpoint,
            CreationInfo = new CreationInfo(user!.Id),
        };
        
        return tire;
    }
    public static Result<AchievementTier> Edit(AchievementTier tier, Value<string> name, int minpoint, int maxpoint)
    {
        var errors = new ValidationErrors();

         if (minpoint < 0)
            errors.Add(name, ValidationError.Create("MinPoints is required", "Error.AchievementTier.MinPointsNonNegative"));

        if (maxpoint <= minpoint)
            errors.Add(name, ValidationError.Create("MaxPoints must be greater than MinPoints", "Error.AchievementTier.MaxPointsInvalid"));

        if (errors.HasErrors)
            return Error.Validation(errors);
        tier.Name = name.Data!;
        tier.MinPoints = minpoint;
        tier.MaxPoints = maxpoint;

        return tier;
    }
}
