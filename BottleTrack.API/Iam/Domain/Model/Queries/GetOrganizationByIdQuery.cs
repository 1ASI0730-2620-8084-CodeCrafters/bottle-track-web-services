namespace BottleTrack.API.Iam.Domain.Model.Queries;

public class GetOrganizationByIdQuery
{
    public Guid OrganizationId { get; }

    public GetOrganizationByIdQuery(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}
