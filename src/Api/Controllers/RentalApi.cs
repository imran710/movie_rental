using Api.Presentation.Common;
using Core.Features.Users.UseCase.GetCurrentUser;

namespace Api.Controllers;

public class RentalApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var userGroup = routes.MapGroup("/rental").WithTags("rental").RequireAuthorization(); 

       /* userGroup
            .MapGet("/v1/current-user", GetCurrentUser)
            .WithSummary("Get current user information");*/
    }

}
