namespace BottleTrack.API.Fleet.Domain.Model.Queries;

public class GetVehiclesByOrganizationIdQuery
{
    public Guid OrganizationId { get; }

    public bool OnlyAvailable { get; }

    public GetVehiclesByOrganizationIdQuery(Guid organizationId, bool onlyAvailable)
    {
        OrganizationId = organizationId;
        OnlyAvailable = onlyAvailable;
    }
}
