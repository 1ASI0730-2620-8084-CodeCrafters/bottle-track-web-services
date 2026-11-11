using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Iam.Interfaces.Rest.Resources;

namespace BottleTrack.API.Iam.Interfaces.Rest.Transform;

public static class AuthenticationAssembler
{
    public static SignInCommand ToCommand(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }

    public static AuthenticatedUserResource ToResource(AuthenticatedUser authenticated)
    {
        AuthenticatedUserResource resource = new AuthenticatedUserResource();
        resource.Id = authenticated.User.Id;
        resource.OrganizationId = authenticated.User.OrganizationId;
        resource.Email = authenticated.User.Email.Value;
        resource.FirstName = authenticated.User.FirstName;
        resource.LastName = authenticated.User.LastName;
        resource.Role = authenticated.User.Role;
        resource.Status = authenticated.User.Status;
        resource.Token = authenticated.Token;
        return resource;
    }
}
