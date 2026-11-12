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
}
