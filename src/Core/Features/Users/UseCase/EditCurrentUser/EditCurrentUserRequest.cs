using Core.Features.Users.ValueObject.PersonalName;

using Microsoft.AspNetCore.Http;

namespace Core.Features.Users.UseCase.EditCurrentUser;

public class EditCurrentUserRequest
{
    public UserPersonalNameModel? PersonalName { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Username { get; set; } = null;
    public IFormFile? UserProfileImage { get; set; } = null;
}
