using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Domain.Repositories;

public interface IIotDeviceRepository : IBaseRepository<IotDevice>
{
    Task<IotDevice?> FindBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default);
}
