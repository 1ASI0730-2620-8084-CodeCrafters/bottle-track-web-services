using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Queries;
using BottleTrack.API.Monitoring.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Application.Internal.QueryServices;

public class SensorReadingQueryService : ISensorReadingQueryService
{
    private readonly ISensorReadingRepository sensorReadingRepository;

    public SensorReadingQueryService(ISensorReadingRepository sensorReadingRepository)
    {
        this.sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<IEnumerable<SensorReading>> Handle(GetSensorReadingsByDeviceIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await sensorReadingRepository.ListByDeviceAsync(query.IotDeviceId, cancellationToken);
    }
}
