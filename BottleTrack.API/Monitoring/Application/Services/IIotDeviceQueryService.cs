using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Queries;

namespace BottleTrack.API.Monitoring.Application.Services;

public interface IIotDeviceQueryService
{
    Task<IotDevice?> Handle(GetIotDeviceByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<IotDevice>> Handle(GetAllIotDevicesQuery query, CancellationToken cancellationToken = default);
}
