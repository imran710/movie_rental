using Core.Features.Users.Entity;

namespace Core.Features.Users.Common;
public class AchievementTierModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinPoints { get; set; }
    public int MaxPoints { get; set; }
    public bool isUserTire { get; set; }

    public static IList<AchievementTierModel> GetAcheventTires(List<AchievementTier> achievementTiers,double userpoints)
    {

        var achievementTiersList = achievementTiers
                                  .Select(a => new AchievementTierModel
                                  {
                                      Id = a.Id,
                                      Name = a.Name,
                                      MinPoints = a.MinPoints,
                                      MaxPoints = a.MaxPoints,
                                      isUserTire = userpoints >= a.MinPoints && userpoints <= a.MaxPoints
                                  })
                                  .ToList();

        return achievementTiersList;
    }
}
