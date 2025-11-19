namespace Core.Features.Users.ValueObject.ContactInfo;

public class UserContactInfoErrors
{
    public static readonly ValidationError InvalidContactNumber
        = ValidationError.Create("Invalid contact number", code: "Error.UserContactInfo.InvalidContactNumber");
}

