using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Operations.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Operations.Application.Internal.CommandServices;

public class TransportOperationCommandService : ITransportOperationCommandService
{
    private readonly ITransportOperationRepository transportOperationRepository;
    private readonly IUnitOfWork unitOfWork;

    public TransportOperationCommandService(ITransportOperationRepository transportOperationRepository,
        IUnitOfWork unitOfWork)
    {
        this.transportOperationRepository = transportOperationRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(
        CreateTransportOperationCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Description))
            return Result<TransportOperation, TransportOperationError>.Failure(
                TransportOperationError.InvalidTransportOperationData);

        int count = await transportOperationRepository.CountAsync(cancellationToken);
        string code = "OP-" + (count + 1).ToString("D5");

        TransportOperation operation = TransportOperation.Create(code, command.OrganizationId, command.Description,
            command.ScheduledDate);

        await transportOperationRepository.AddAsync(operation, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<TransportOperation, TransportOperationError>.Success(operation);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(AddDeliveryStopCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.PlannedQuantity <= 0 || command.SequenceNumber <= 0)
            return Result<TransportOperation, TransportOperationError>.Failure(
                TransportOperationError.InvalidTransportOperationData);

        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Result<TransportOperation, TransportOperationError>.Failure(
                TransportOperationError.TransportOperationNotFound);

        operation.AddStop(command.DeliveryPointId, command.SequenceNumber, command.PlannedQuantity);

        transportOperationRepository.Update(operation);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<TransportOperation, TransportOperationError>.Success(operation);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(
        DispatchTransportOperationCommand command, CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Result<TransportOperation, TransportOperationError>.Failure(
                TransportOperationError.TransportOperationNotFound);

        operation.Dispatch(command.VehicleId, command.DriverId);

        transportOperationRepository.Update(operation);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<TransportOperation, TransportOperationError>.Success(operation);
    }
}
