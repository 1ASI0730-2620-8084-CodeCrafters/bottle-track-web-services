using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Iam.Application.Services;

public interface IOrganizationCommandService
{
    Task<Result<Organization, OrganizationError>> Handle(RegisterOrganizationCommand command,
        CancellationToken cancellationToken = default);
}
