using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Domain.Repositories;

public interface ISensorReadingRepository : IBaseRepository<SensorReading>
{
    Task<IEnumerable<SensorReading>> ListByDeviceAsync(Guid iotDeviceId, CancellationToken cancellationToken = default);
}
