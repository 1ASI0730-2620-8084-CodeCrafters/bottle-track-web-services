using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Queries;

namespace BottleTrack.API.Fleet.Application.Services;

public interface IVehicleQueryService
{
    Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<Vehicle>> Handle(GetVehiclesByOrganizationIdQuery query,
        CancellationToken cancellationToken = default);
}
