using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Queries;

namespace BottleTrack.API.Monitoring.Application.Services;

public interface ISensorReadingQueryService
{
    Task<IEnumerable<SensorReading>> Handle(GetSensorReadingsByDeviceIdQuery query,
        CancellationToken cancellationToken = default);
}
