using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Queries;
using BottleTrack.API.Monitoring.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Application.Internal.QueryServices;

public class AlertQueryService : IAlertQueryService
{
    private readonly IAlertRepository alertRepository;

    public AlertQueryService(IAlertRepository alertRepository)
    {
        this.alertRepository = alertRepository;
    }

    public async Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await alertRepository.FindByIdAsync(query.AlertId, cancellationToken);
    }

    public async Task<IEnumerable<Alert>> Handle(GetAlertsByDeviceIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await alertRepository.ListByDeviceAsync(query.IotDeviceId, cancellationToken);
    }
}
