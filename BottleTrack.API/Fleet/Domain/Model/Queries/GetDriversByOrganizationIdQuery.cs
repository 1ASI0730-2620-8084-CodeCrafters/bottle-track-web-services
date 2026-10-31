namespace BottleTrack.API.Fleet.Domain.Model.Queries;

public class GetDriversByOrganizationIdQuery
{
    public Guid OrganizationId { get; }

    public bool OnlyAvailable { get; }

    public GetDriversByOrganizationIdQuery(Guid organizationId, bool onlyAvailable)
    {
        OrganizationId = organizationId;
        OnlyAvailable = onlyAvailable;
    }
}
