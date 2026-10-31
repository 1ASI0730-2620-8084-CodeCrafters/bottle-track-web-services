using BottleTrack.API.Fleet.Application.Errors;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Fleet.Application.Services;

public interface IDriverCommandService
{
    Task<Result<Driver, DriverError>> Handle(RegisterDriverCommand command,
        CancellationToken cancellationToken = default);
}
