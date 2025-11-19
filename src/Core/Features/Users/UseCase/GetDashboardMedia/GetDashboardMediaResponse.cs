using Core.Features.Users.Model;

namespace Core.Features.Users.UseCase.GetDashboardMedia;
public record GetDashboardMediaResponse(IList<DashboardFileModel>? dashboardImages, IList<DashboardFileModel>? dashboardGifs);
