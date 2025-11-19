using PhoneNumbers;

namespace Core.Features.Users.ValueObject.ContactInfo;

public record UserContactInfo
{
    public const int MaxLength = 100;

    public string RegionCode { get; }
    public string PhoneNumber { get; }
    public bool PhoneNumberConfirmed { get; private set; }

    private UserContactInfo(string regionCode, string phoneNumber, bool phoneNumberConfirmed = false)
    {
        RegionCode = regionCode;
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = phoneNumberConfirmed;
    }

    public static UserContactInfo NoContactInfo => new(string.Empty, string.Empty);

    public static Result<UserContactInfo> Create(Value<string> phoneNumber)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber.Data, null);
        var isValid = phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        if (!isValid)
        {
            return Error.Validation(phoneNumber, UserContactInfoErrors.InvalidContactNumber);
        }

        var regionCode = phoneNumberUtil.GetRegionCodeForNumber(parsedPhoneNumber);
        var formattedPhoneNumber = phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.INTERNATIONAL);
        return new UserContactInfo(regionCode, formattedPhoneNumber);
    }

    public void Confirm()
    {
        if (PhoneNumberConfirmed)
            throw new InvalidOperationException("Phone number is already confirmed");

        PhoneNumberConfirmed = true;
    }

    public override string ToString()
    {
        return PhoneNumber == string.Empty
            ? "No Contact Information"
            : $"Phone: {PhoneNumber}, Confirmed: {PhoneNumberConfirmed}";
    }
}

