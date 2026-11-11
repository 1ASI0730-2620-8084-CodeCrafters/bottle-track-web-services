using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Monitoring.Application.Services;

public interface IAlertCommandService
{
    Task<Result<Alert, AlertError>> Handle(RaiseAlertCommand command, CancellationToken cancellationToken = default);

    Task<Result<Alert, AlertError>> Handle(AcknowledgeAlertCommand command,
        CancellationToken cancellationToken = default);
}
