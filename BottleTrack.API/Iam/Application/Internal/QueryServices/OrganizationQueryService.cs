using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Queries;
using BottleTrack.API.Iam.Domain.Repositories;

namespace BottleTrack.API.Iam.Application.Internal.QueryServices;

public class OrganizationQueryService : IOrganizationQueryService
{
    private readonly IOrganizationRepository organizationRepository;

    public OrganizationQueryService(IOrganizationRepository organizationRepository)
    {
        this.organizationRepository = organizationRepository;
    }

    public async Task<Organization?> Handle(GetOrganizationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await organizationRepository.FindByIdAsync(query.OrganizationId, cancellationToken);
    }
}
