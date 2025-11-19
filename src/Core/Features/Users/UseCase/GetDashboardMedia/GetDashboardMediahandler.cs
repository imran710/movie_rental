using Core.Domain.Common;
using Core.Domain.Parser;
using Core.Features.Users.Entity;
using Core.Features.Users.Model;
using Core.Infrastructure.Common.Enums;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.GetDashboardMedia;
public class GetDashboardMediahandler(AppDbContext appDbContext) : BaseHandler<GetDashboardMediaRequest, GetDashboardMediaResponse>
{

    protected override async Task<Result<GetDashboardMediaResponse>> Handle(GetDashboardMediaRequest request, CancellationToken cancellationToken = default)
    {
        var requestEnums = request.Include.ParseIncludeString<DashboardFileType>();
        var currentUser = _httpContext.GetCurrentUser();
        var user = await appDbContext.Users
                                      .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken)
                                      .ConfigureAwait(false);

        if (user == null)
            return Error.NotFound("Current user information is not found");
        user = await appDbContext.Users.FindById(currentUser.UserId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (user is null)
            return Error.NotFound();

        var dashboard = await appDbContext.DashboardFiles
                     .ToArrayAsync(cancellationToken);
        IList<DashboardFileModel>? dashboardImages = null;    
        IList<DashboardFileModel>? dashboardGifs = null;    
        if (requestEnums.Contains(DashboardFileType.Image))
        {
            dashboardImages= DashboardFileModel.GetFileList(dashboard, user.Id, DashboardFileType.Image);
        }
        if (requestEnums.Contains(DashboardFileType.Gif))
        {
            dashboardImages = DashboardFileModel.GetFileList(dashboard, user.Id, DashboardFileType.Gif);
        }

         return new GetDashboardMediaResponse(dashboardImages, dashboardImages);
       
    }
}
