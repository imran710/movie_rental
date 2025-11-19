using Api.Presentation.Common;
using Api.Presentation.Response;

using Core.Features.Users.GetRoles;
using Core.Features.Users.UseCase.CreateAdminUser;
using Core.Features.Users.UseCase.DeleteAccount;
using Core.Features.Users.UseCase.EditCurrentUser;
using Core.Features.Users.UseCase.EditUserPassword;
using Core.Features.Users.UseCase.GetCurrentUser;
using Core.Features.Users.UseCase.GetDashboardMedia;
using Core.Features.Users.UseCase.GetUserByRole;
using Core.Features.Users.UseCase.SocialLogin;
using Core.Infrastructure.Common.Enums;
using Core.Infrastructure.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var userGroup = routes.MapGroup("/users").WithTags("User").RequireAuthorization();
        var socialGroup = routes.MapGroup("/users-social").WithTags("User");

        userGroup
            .MapGet("/v1/current-user", GetCurrentUser)
            .WithSummary("Get current user information")
            .WithDescription($"""
                {EnumExtensions.ToAttributeString<IncludeRequestEnum>("include")}
                note: add string values with commas.
                example: HealthProfile,Profile,Points,Subscription
                """);

        userGroup
            .MapPost("/v1/edit/current-user", EditCurrentUser)
            .WithSummary("Edit current user information")
            .DisableAntiforgery();

        userGroup
            .MapPost("/v1/edit/current-user-password", EditCurrentUserPassword)
            .WithSummary("Edit current user password");

        userGroup
            .MapGet("/v1/user/user-dashboard-files", GetDashboardMediaList)
            .WithSummary("Get current user dashboard files")
            .WithDescription($"""
                {EnumExtensions.ToAttributeString<DashboardFileType>()}
                note: add string values with commas.
                example: Image,Gif
                """);

        userGroup
            .MapPost("/v1/delete/current-user", DeleteAccount)
            .WithSummary("Delete account");

     
        userGroup
            .MapGet("/v1/user/get-roles", GetRoles)
            .WithSummary("Get roles");

        userGroup
            .MapGet("/v1/user/user-by-role", GetUserByRole)
            .WithSummary("Get  users by role id");

        socialGroup
           .MapPost("/v1/login/social-login", SocialLogin)
           .WithSummary("Log in with social media");

        userGroup
            .MapPost("/v1/user/create-admin-user", CreateAdminUser)
            .WithSummary("Create admin user");
    }

    private static async Task<Results<Ok<ApiResponse<GetCurrentUserResponse>>, JsonHttpResult<ApiResponse<GetCurrentUserResponse>>>> GetCurrentUser(
        [FromQuery] string? include,
        [FromServices] GetCurrentUserHandler emailLoginHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var request = new GetCurrentUserRequest(include);

        var result = await emailLoginHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<EditCurrentUserResponse>>, JsonHttpResult<ApiResponse<EditCurrentUserResponse>>>> EditCurrentUser(
        [FromForm] EditCurrentUserRequest request,
        [FromServices] EditCurrentUserHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<EditUserPasswordResponse>>, JsonHttpResult<ApiResponse<EditUserPasswordResponse>>>> EditCurrentUserPassword(
        [FromBody] EditUserPasswordRequest request,
        [FromServices] EditUserPasswordHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<GetDashboardMediaResponse>>, JsonHttpResult<ApiResponse<GetDashboardMediaResponse>>>> GetDashboardMediaList(
        [FromQuery] string? include,
        [FromServices] GetDashboardMediahandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var request = new GetDashboardMediaRequest(include);
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<DeleteAccountResponse>>, JsonHttpResult<ApiResponse<DeleteAccountResponse>>>> DeleteAccount(
        [FromBody] DeleteAccountRequest request,
        [FromServices] DeleteAccountHander handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

 
    private static async Task<Results<Ok<ApiResponse<SocialLoginResponse>>, JsonHttpResult<ApiResponse<SocialLoginResponse>>>> SocialLogin(
        [FromBody] SocialLoginRequest request,
        [FromServices] SocialLoginHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<GetUsersByRoleResponse>>, JsonHttpResult<ApiResponse<GetUsersByRoleResponse>>>> GetUserByRole(
        [FromQuery] long? roleId,
        [FromServices] GetUsersByRoleHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var request = new GetUsersByRoleRequest(roleId);
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<GetRolesResponse>>, JsonHttpResult<ApiResponse<GetRolesResponse>>>> GetRoles(
        [FromServices] GetRolesHandller handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(new GetRolesRequest(), httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<CreateAdminUserResponse>>, JsonHttpResult<ApiResponse<CreateAdminUserResponse>>>> CreateAdminUser(
        [FromBody] CreateAdminUserRequest request,
        [FromServices] CreateAdminUserHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }
}
