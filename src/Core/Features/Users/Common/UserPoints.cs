using Core.Features.Users.Entity;

namespace Core.Features.Users.Common;
public class UserPoints
{
    public double Points { get; set; }
    public string Tier { get; set; } = string.Empty;

    public static UserPoints? GetUserPoints(List<UserAchievement>? userAchievements, long userId, List<AchievementTier> achievementTiers)
    {
        if (userAchievements == null || !userAchievements.Any())
            return null; 

        var userAchievementsForUser = userAchievements
            .Where(ua => ua.UserId == userId)
            .ToList();

        if (!userAchievementsForUser.Any())
            return null;

        var totalPoints = userAchievementsForUser.Sum(ua => ua.PointsEarned);

        var userPoints = achievementTiers
            .Where(tier => totalPoints >= tier.MinPoints && totalPoints <= tier.MaxPoints)
            .OrderByDescending(tier => tier.MinPoints)
            .Select(tier => new UserPoints
            {
                Tier = tier.Name,
                Points = totalPoints
            })
            .FirstOrDefault() ?? new UserPoints { Tier = "Unranked", Points = totalPoints }; 

        return userPoints;
    }

}

