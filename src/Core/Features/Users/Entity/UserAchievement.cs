using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Domain.Helper;

namespace Core.Features.Users.Entity;
public class UserAchievement : IEntity, IDeletableEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
    public long TaskId { get; set; } 
    public double PointsEarned { get; set; }
    public DateTimeOffset DateEarned { get; set; }
    public long AchievementTierId { get; set; }
    public AchievementTier? AchievementTier { get; set; } 
    public required CreationInfo CreationInfo { get; init; }
    public DeletionInfo DeletionInfo { get; } = DeletionInfo.NoDeleted();

    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }
    public static UserAchievement Create(long userId, double pointsEarned, long achievementTierId,long taskid)
    {
        return new UserAchievement
        {
            UserId = userId,
            PointsEarned = pointsEarned,
            TaskId= taskid,
            DateEarned = DateTimeHelperStatic.Now,
            AchievementTierId = achievementTierId,
            CreationInfo = new CreationInfo(userId)
        };
    }
}
