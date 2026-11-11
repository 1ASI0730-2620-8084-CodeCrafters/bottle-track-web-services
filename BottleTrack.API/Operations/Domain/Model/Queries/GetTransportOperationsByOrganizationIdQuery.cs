namespace BottleTrack.API.Operations.Domain.Model.Queries;

public class GetTransportOperationsByOrganizationIdQuery
{
    public Guid OrganizationId { get; }

    public GetTransportOperationsByOrganizationIdQuery(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}
