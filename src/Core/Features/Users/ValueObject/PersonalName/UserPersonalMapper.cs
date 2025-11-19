namespace Core.Features.Users.ValueObject.PersonalName;

public static class UserPersonalMapper
{
    public static UserPersonalNameModel MapToUserPersonalNameModel(this UserPersonalName userPersonalName)
    {
        return new UserPersonalNameModel
        {
            FirstName = userPersonalName.FirstName,
            LastName = userPersonalName.LastName,
        };
    }

    public static Result<UserPersonalName> MapToUserPersonalName(this UserPersonalNameModel personalNameModel)
    {
        return UserPersonalName.Create(Value.New(personalNameModel.FirstName), Value.New(personalNameModel.LastName));
    }
}
