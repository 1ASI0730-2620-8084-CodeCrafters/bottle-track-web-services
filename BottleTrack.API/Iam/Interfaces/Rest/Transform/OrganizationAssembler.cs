using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Iam.Interfaces.Rest.Resources;

namespace BottleTrack.API.Iam.Interfaces.Rest.Transform;

public static class OrganizationAssembler
{
    public static RegisterOrganizationCommand ToCommand(RegisterOrganizationResource resource)
    {
        return new RegisterOrganizationCommand(resource.BusinessName, resource.TaxId, resource.Email, resource.Phone,
            resource.Address);
    }

    public static UpdateOrganizationCommand ToCommand(Guid id, UpdateOrganizationResource resource)
    {
        return new UpdateOrganizationCommand(id, resource.BusinessName, resource.Phone, resource.Address);
    }

    public static OrganizationResource ToResource(Organization organization)
    {
        OrganizationResource resource = new OrganizationResource();
        resource.Id = organization.Id;
        resource.BusinessName = organization.BusinessName;
        resource.TaxId = organization.TaxId.Value;
        resource.Email = organization.Email.Value;
        resource.Phone = organization.Phone;
        resource.Address = organization.Address;
        return resource;
    }
}
