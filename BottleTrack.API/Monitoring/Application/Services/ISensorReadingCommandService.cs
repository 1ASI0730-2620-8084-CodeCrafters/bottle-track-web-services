using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Monitoring.Application.Services;

public interface ISensorReadingCommandService
{
    Task<Result<SensorReading, SensorReadingError>> Handle(RecordSensorReadingCommand command,
        CancellationToken cancellationToken = default);
}
