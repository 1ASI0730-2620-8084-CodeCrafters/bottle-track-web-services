using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> ListByDeviceAsync(Guid iotDeviceId, CancellationToken cancellationToken = default);
}
