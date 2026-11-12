using BottleTrack.API.Fleet.Application.Errors;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Fleet.Application.Services;

public interface IVehicleCommandService
{
    Task<Result<Vehicle, VehicleError>> Handle(RegisterVehicleCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<Vehicle, VehicleError>> Handle(UpdateVehicleCommand command,
        CancellationToken cancellationToken = default);
}
