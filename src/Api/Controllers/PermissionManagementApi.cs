using Api.Presentation.Common;
using Api.Presentation.Response;

using Core.Features.PermissionManagement.UseCase.AssignPermissions;
using Core.Features.PermissionManagement.UseCase.AssignRole;
using Core.Features.PermissionManagement.UseCase.CheckUserPermissions;
using Core.Features.PermissionManagement.UseCase.CreateRole;
using Core.Features.PermissionManagement.UseCase.EditPermission;
using Core.Features.PermissionManagement.UseCase.EditRole;
using Core.Features.PermissionManagement.UseCase.GetPermissions;
using Core.Features.PermissionManagement.UseCase.GetRoles;
using Core.Features.PermissionManagement.UseCase.GetSubscriptionPermissions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PermissionManagementApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/permission-management").WithTags("Permission Management").RequireAuthorization();

        group.MapGet("/permission/get", GetPermissions).WithSummary("Get all permissions");
        group.MapPost("/permission/edit", EditPermission).WithSummary("Edit a permission");
        group.MapGet("/role/get", GetRoles).WithSummary("Get all roles");
        group.MapPost("/role/create", CreateRole).WithSummary("Create a new role");
        group.MapPost("/role/edit", EditRole).WithSummary("Edit a role");

        group.MapPost("/role/assign-permissions", AssignPermissions).WithSummary("Assign permissions to a role");
        group.MapPost("/role/assign-role", AssignRole).WithSummary("Assign a role to a user");
        group.MapGet("/subscription/permissions", GetSubscriptionPermissions).WithSummary("Get all subscription permissions");
        group.MapPost("/user/check-permissions", CheckUserPermissions).WithSummary("Check user permissions");
    }

    private static async Task<Results<Ok<ApiResponse<GetPermissionsResponse>>, JsonHttpResult<ApiResponse<GetPermissionsResponse>>>> GetPermissions(
        [AsParameters] GetPermissionsRequest request,
        [FromServices] GetPermissionsHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<EditPermissionResponse>>, JsonHttpResult<ApiResponse<EditPermissionResponse>>>> EditPermission(
        [FromBody] EditPermissionRequest request,
        [FromServices] EditPermissionHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<GetRolesResponse>>, JsonHttpResult<ApiResponse<GetRolesResponse>>>> GetRoles(
        [AsParameters] GetRolesRequest request,
        [FromServices] GetRolesHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<CreateRoleResponse>>, JsonHttpResult<ApiResponse<CreateRoleResponse>>>> CreateRole(
        [FromBody] CreateRoleRequest request,
        [FromServices] CreateRoleHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<EditRoleResponse>>, JsonHttpResult<ApiResponse<EditRoleResponse>>>> EditRole(
        [FromBody] EditRoleRequest request,
        [FromServices] EditRoleHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<AssignPermissionsResponse>>, JsonHttpResult<ApiResponse<AssignPermissionsResponse>>>> AssignPermissions(
        [FromBody] AssignPermissionsRequest request,
        [FromServices] AssignPermissionsHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<AssignRoleResponse>>, JsonHttpResult<ApiResponse<AssignRoleResponse>>>> AssignRole(
        [FromBody] AssignRoleRequest request,
        [FromServices] AssignRoleHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<GetSubscriptionPermissionResponse>>, JsonHttpResult<ApiResponse<GetSubscriptionPermissionResponse>>>> GetSubscriptionPermissions(
        [FromServices] GetSubscriptionPermissionHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(new GetSubscriptionPermissionRequest(), httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<CheckUserPermissionsResponse>>, JsonHttpResult<ApiResponse<CheckUserPermissionsResponse>>>> CheckUserPermissions(
        [FromBody] CheckUserPermissionsRequest request,
        [FromServices] CheckUserPermissionsHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }
}
