namespace BottleTrack.API.Iam.Domain.Model.Queries;

public class GetUsersByOrganizationIdQuery
{
    public Guid OrganizationId { get; }

    public GetUsersByOrganizationIdQuery(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}
