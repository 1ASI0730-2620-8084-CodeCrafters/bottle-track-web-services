using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Queries;

namespace BottleTrack.API.Iam.Application.Services;

public interface IOrganizationQueryService
{
    Task<Organization?> Handle(GetOrganizationByIdQuery query, CancellationToken cancellationToken = default);
}
