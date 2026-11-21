using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Operations.Application.Services;

public interface ITransportOperationCommandService
{
    Task<Result<TransportOperation, TransportOperationError>> Handle(CreateTransportOperationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(AddDeliveryStopCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(DispatchTransportOperationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(MoveDeliveryStopCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(AssignVehicleCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(AssignDriverCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(StartTransportOperationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(RecordDeliveryCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(CompleteTransportOperationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<TransportOperation, TransportOperationError>> Handle(CancelTransportOperationCommand command,
        CancellationToken cancellationToken = default);
}
