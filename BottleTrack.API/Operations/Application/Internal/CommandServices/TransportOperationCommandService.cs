using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Application.Internal.OutboundServices;
using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Operations.Domain.Model.Entities;
using BottleTrack.API.Operations.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Operations.Application.Internal.CommandServices;

public class TransportOperationCommandService : ITransportOperationCommandService
{
    private readonly ITransportOperationRepository transportOperationRepository;
    private readonly ExternalFleetService externalFleetService;
    private readonly IUnitOfWork unitOfWork;

    public TransportOperationCommandService(ITransportOperationRepository transportOperationRepository,
        ExternalFleetService externalFleetService, IUnitOfWork unitOfWork)
    {
        this.transportOperationRepository = transportOperationRepository;
        this.externalFleetService = externalFleetService;
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

        if (!operation.CanChangeResources)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        operation.AddStop(command.DeliveryPointId, command.SequenceNumber, command.PlannedQuantity);

        transportOperationRepository.Update(operation);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<TransportOperation, TransportOperationError>.Success(operation);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(MoveDeliveryStopCommand command,
        CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (operation.FindStop(command.DeliveryStopId) == null)
            return Failure(TransportOperationError.DeliveryStopNotFound);

        if (!operation.CanMoveStop(command.DeliveryStopId, command.Offset))
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        operation.MoveStop(command.DeliveryStopId, command.Offset);
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(AssignVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanChangeResources)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        bool isReserved = await externalFleetService.SwapVehicleAsync(operation.VehicleId, command.VehicleId,
            cancellationToken);
        if (!isReserved)
            return Failure(TransportOperationError.VehicleNotAvailable);

        operation.AssignVehicle(command.VehicleId);
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(AssignDriverCommand command,
        CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanChangeResources)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        bool isReserved = await externalFleetService.SwapDriverAsync(operation.DriverId, command.DriverId,
            cancellationToken);
        if (!isReserved)
            return Failure(TransportOperationError.DriverNotAvailable);

        operation.AssignDriver(command.DriverId);
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(
        DispatchTransportOperationCommand command, CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanChangeResources || operation.DeliveryStops.Count == 0)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        bool isVehicleReserved = await externalFleetService.SwapVehicleAsync(operation.VehicleId,
            command.VehicleId, cancellationToken);
        if (!isVehicleReserved)
            return Failure(TransportOperationError.VehicleNotAvailable);

        bool isDriverReserved = await externalFleetService.SwapDriverAsync(operation.DriverId, command.DriverId,
            cancellationToken);
        if (!isDriverReserved)
            return Failure(TransportOperationError.DriverNotAvailable);

        operation.Dispatch(command.VehicleId, command.DriverId);
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(
        StartTransportOperationCommand command, CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanStart)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        operation.Start();
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(RecordDeliveryCommand command,
        CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanRecordDeliveries)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        DeliveryStop? deliveryStop = operation.FindStop(command.DeliveryStopId);
        if (deliveryStop == null)
            return Failure(TransportOperationError.DeliveryStopNotFound);

        if (!deliveryStop.IsValidResult(command.Status, command.DeliveredQuantity, command.FailureReason))
            return Failure(TransportOperationError.InvalidDeliveryResult);

        operation.RecordDelivery(command.DeliveryStopId, command.Status, command.DeliveredQuantity,
            command.FailureReason);
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(
        CompleteTransportOperationCommand command, CancellationToken cancellationToken = default)
    {
        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanComplete)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        operation.Complete();
        await externalFleetService.ReleaseResourcesAsync(operation.VehicleId, operation.DriverId, cancellationToken);
        return await Save(operation, cancellationToken);
    }

    public async Task<Result<TransportOperation, TransportOperationError>> Handle(
        CancelTransportOperationCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Reason))
            return Failure(TransportOperationError.InvalidTransportOperationData);

        TransportOperation? operation = await transportOperationRepository.FindByIdWithStopsAsync(
            command.TransportOperationId, cancellationToken);
        if (operation == null)
            return Failure(TransportOperationError.TransportOperationNotFound);

        if (!operation.CanCancel)
            return Failure(TransportOperationError.InvalidTransportOperationStatusChange);

        operation.Cancel(command.Reason);
        await externalFleetService.ReleaseResourcesAsync(operation.VehicleId, operation.DriverId, cancellationToken);
        return await Save(operation, cancellationToken);
    }

    private async Task<Result<TransportOperation, TransportOperationError>> Save(TransportOperation operation,
        CancellationToken cancellationToken)
    {
        transportOperationRepository.Update(operation);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<TransportOperation, TransportOperationError>.Success(operation);
    }

    private static Result<TransportOperation, TransportOperationError> Failure(TransportOperationError error)
    {
        return Result<TransportOperation, TransportOperationError>.Failure(error);
    }
}
