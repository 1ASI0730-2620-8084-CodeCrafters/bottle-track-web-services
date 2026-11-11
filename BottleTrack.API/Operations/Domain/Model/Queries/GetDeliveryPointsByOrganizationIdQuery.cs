namespace BottleTrack.API.Operations.Domain.Model.Queries;

public class GetDeliveryPointsByOrganizationIdQuery
{
    public Guid OrganizationId { get; }

    public GetDeliveryPointsByOrganizationIdQuery(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}
