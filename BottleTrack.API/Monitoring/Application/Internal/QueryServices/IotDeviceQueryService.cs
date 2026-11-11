using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Queries;
using BottleTrack.API.Monitoring.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Application.Internal.QueryServices;

public class IotDeviceQueryService : IIotDeviceQueryService
{
    private readonly IIotDeviceRepository iotDeviceRepository;

    public IotDeviceQueryService(IIotDeviceRepository iotDeviceRepository)
    {
        this.iotDeviceRepository = iotDeviceRepository;
    }

    public async Task<IotDevice?> Handle(GetIotDeviceByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await iotDeviceRepository.FindByIdAsync(query.IotDeviceId, cancellationToken);
    }

    public async Task<IEnumerable<IotDevice>> Handle(GetAllIotDevicesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await iotDeviceRepository.ListAsync(cancellationToken);
    }
}
