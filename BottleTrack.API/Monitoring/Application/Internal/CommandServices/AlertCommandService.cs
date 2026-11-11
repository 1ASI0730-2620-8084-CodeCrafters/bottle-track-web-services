using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Application.Internal.CommandServices;

public class AlertCommandService : IAlertCommandService
{
    private readonly IAlertRepository alertRepository;
    private readonly IUnitOfWork unitOfWork;

    public AlertCommandService(IAlertRepository alertRepository, IUnitOfWork unitOfWork)
    {
        this.alertRepository = alertRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Alert, AlertError>> Handle(RaiseAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        Alert alert = Alert.Raise(command.IotDeviceId, command.TransportOperationId, command.Type);

        await alertRepository.AddAsync(alert, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Alert, AlertError>.Success(alert);
    }

    public async Task<Result<Alert, AlertError>> Handle(AcknowledgeAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        Alert? alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert == null)
            return Result<Alert, AlertError>.Failure(AlertError.AlertNotFound);

        alert.Acknowledge(command.AcknowledgedBy);

        alertRepository.Update(alert);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Alert, AlertError>.Success(alert);
    }
}
