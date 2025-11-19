using Core.Features.Users.Entity;
using Core.Infrastructure.Common.Enums;

namespace Core.Features.Users.Model;
public class DashboardFileModel
{
    public long Id { get; set; }
    public required long UserId { get; init; }
    public string CreatedBy { get; set; }=string.Empty;
    public required string FileUrl { get; set; }
    public DashboardFileType FileType { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public static IList<DashboardFileModel> GetFileList(DashboardFile[] dashboardFiles, long userId, DashboardFileType fileType)
    {
        var dashboardData = dashboardFiles
            .Where(t => t.UserId == userId && t.FileType== fileType && !t.DeletionInfo.IsDeleted) 
            .Select(t => new DashboardFileModel
            {
                Id = t.Id,
                UserId = t.UserId,
                CreatedTime = t.CreationInfo?.CreatedAt ?? DateTimeOffset.MinValue,
                CreatedBy = t.User?.PersonalName?.FullName?? "",
                FileUrl = t.FileUrl,
                FileType = t.FileType,
                
            }).ToList();

        return dashboardData;
    }
}

