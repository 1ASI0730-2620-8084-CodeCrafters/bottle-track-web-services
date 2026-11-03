using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Iam.Interfaces.Rest.Resources;

namespace BottleTrack.API.Iam.Interfaces.Rest.Transform;

public static class UserAssembler
{
    public static RegisterUserCommand ToCommand(RegisterUserResource resource)
    {
        return new RegisterUserCommand(resource.OrganizationId, resource.FirstName, resource.LastName, resource.Email,
            resource.Password, resource.Role);
    }

    public static UserResource ToResource(User user)
    {
        UserResource resource = new UserResource();
        resource.Id = user.Id;
        resource.OrganizationId = user.OrganizationId;
        resource.FirstName = user.FirstName;
        resource.LastName = user.LastName;
        resource.Email = user.Email.Value;
        resource.Role = user.Role;
        resource.Status = user.Status;
        return resource;
    }
}
