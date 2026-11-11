using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Queries;

namespace BottleTrack.API.Monitoring.Application.Services;

public interface IAlertQueryService
{
    Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<Alert>> Handle(GetAlertsByDeviceIdQuery query, CancellationToken cancellationToken = default);
}
