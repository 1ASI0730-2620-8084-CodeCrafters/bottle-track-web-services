namespace BottleTrack.API.Iam.Domain.Model.Commands;

public class UpdateOrganizationCommand
{
    public Guid OrganizationId { get; }

    public string BusinessName { get; }

    public string Phone { get; }

    public string Address { get; }

    public UpdateOrganizationCommand(Guid organizationId, string businessName, string phone, string address)
    {
        OrganizationId = organizationId;
        BusinessName = businessName;
        Phone = phone;
        Address = address;
    }
}
