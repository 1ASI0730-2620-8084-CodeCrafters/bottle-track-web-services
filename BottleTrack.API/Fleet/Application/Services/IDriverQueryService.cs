using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Queries;

namespace BottleTrack.API.Fleet.Application.Services;

public interface IDriverQueryService
{
    Task<Driver?> Handle(GetDriverByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<Driver>> Handle(GetDriversByOrganizationIdQuery query,
        CancellationToken cancellationToken = default);
}
