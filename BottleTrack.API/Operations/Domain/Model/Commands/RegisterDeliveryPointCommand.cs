namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class RegisterDeliveryPointCommand
{
    public Guid OrganizationId { get; }

    public Guid? OwnerUserId { get; }

    public string BusinessName { get; }

    public string Address { get; }

    public string District { get; }

    public decimal? Latitude { get; }

    public decimal? Longitude { get; }

    public RegisterDeliveryPointCommand(Guid organizationId, Guid? ownerUserId, string businessName, string address,
        string district, decimal? latitude, decimal? longitude)
    {
        OrganizationId = organizationId;
        OwnerUserId = ownerUserId;
        BusinessName = businessName;
        Address = address;
        District = district;
        Latitude = latitude;
        Longitude = longitude;
    }
}
