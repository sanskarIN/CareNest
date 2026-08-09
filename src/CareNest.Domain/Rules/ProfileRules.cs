using CareNest.Domain.Entities;
using CareNest.Shared;

namespace CareNest.Domain.Rules;

public static class ProfileRules
{
    public static void Validate(PersonProfile profile)
    {
        profile.Name = Guard.NotBlank(profile.Name, nameof(profile.Name), 120);

        if (profile.DateOfBirth is { } dob && dob.Date > DateTime.Today)
        {
            throw new ArgumentException("Date of birth cannot be in the future.", nameof(profile.DateOfBirth));
        }

        if (profile.BloodGroup?.Length > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(profile.BloodGroup));
        }
    }
}
