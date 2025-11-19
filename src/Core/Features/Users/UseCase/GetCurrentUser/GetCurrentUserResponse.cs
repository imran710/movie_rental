using System.Text.Json.Serialization;

using Core.Features.PermissionManagement.Common.Model;
using Core.Features.Users.Common;

namespace Core.Features.Users.UseCase.GetCurrentUser;

public record GetCurrentUserResponse(
    UserModel? User = null, 
    List<RoleModel>? Roles = null);
