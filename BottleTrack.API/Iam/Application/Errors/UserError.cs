namespace BottleTrack.API.Iam.Application.Errors;

public enum UserError
{
    InvalidUserData,
    InvalidEmail,
    PasswordTooShort,
    OrganizationNotFound,
    EmailAlreadyRegistered,
    UserNotFound,
    InvalidCredentials
}
