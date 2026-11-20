using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Entities;
using BottleTrack.API.Operations.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class TransportOperationTests
{
    private static TransportOperation NewOperationWithStop()
    {
        TransportOperation operation = TransportOperation.Create("OP-00001", Guid.NewGuid(), "Lima north route",
            new DateOnly(2026, 11, 20));
        operation.AddStop(Guid.NewGuid(), 1, 50);
        return operation;
    }

    private static TransportOperation NewStartedOperation()
    {
        TransportOperation operation = NewOperationWithStop();
        operation.AssignVehicle(Guid.NewGuid());
        operation.AssignDriver(Guid.NewGuid());
        operation.Start();
        return operation;
    }

    [Fact]
    public void Start_FailsWithoutVehicleAndDriver()
    {
        TransportOperation operation = NewOperationWithStop();

        Assert.False(operation.CanStart);
        Assert.Throws<InvalidOperationException>(() => operation.Start());
    }

    [Fact]
    public void Start_MovesOperationToInProgress()
    {
        TransportOperation operation = NewStartedOperation();

        Assert.Equal(TransportOperationStatus.InProgress, operation.Status);
        Assert.NotNull(operation.DispatchedAt);
    }

    [Fact]
    public void AssignVehicle_FailsOnceOperationStarted()
    {
        TransportOperation operation = NewStartedOperation();

        Assert.Throws<InvalidOperationException>(() => operation.AssignVehicle(Guid.NewGuid()));
    }

    [Fact]
    public void Complete_FailsWhileAStopIsPending()
    {
        TransportOperation operation = NewStartedOperation();

        Assert.False(operation.CanComplete);
        Assert.Throws<InvalidOperationException>(() => operation.Complete());
    }

    [Fact]
    public void RecordDelivery_ThenComplete_FinishesOperation()
    {
        TransportOperation operation = NewStartedOperation();
        DeliveryStop deliveryStop = operation.DeliveryStops[0];

        operation.RecordDelivery(deliveryStop.Id, DeliveryStopStatus.Delivered, 0, null);
        operation.Complete();

        Assert.Equal(DeliveryStopStatus.Delivered, deliveryStop.Status);
        Assert.Equal(50, deliveryStop.DeliveredQuantity);
        Assert.Equal(TransportOperationStatus.Completed, operation.Status);
    }

    [Fact]
    public void RecordDelivery_PartialNeedsReasonAndLowerQuantity()
    {
        TransportOperation operation = NewStartedOperation();
        DeliveryStop deliveryStop = operation.DeliveryStops[0];

        Assert.False(deliveryStop.IsValidResult(DeliveryStopStatus.PartiallyDelivered, 30, null));
        Assert.False(deliveryStop.IsValidResult(DeliveryStopStatus.PartiallyDelivered, 50, "Damaged boxes"));
        Assert.True(deliveryStop.IsValidResult(DeliveryStopStatus.PartiallyDelivered, 30, "Damaged boxes"));
    }

    [Fact]
    public void Cancel_FailsOnceOperationStarted()
    {
        TransportOperation operation = NewStartedOperation();

        Assert.False(operation.CanCancel);
        Assert.Throws<InvalidOperationException>(() => operation.Cancel("Client request"));
    }

    [Fact]
    public void Cancel_KeepsReasonOnPendingOperation()
    {
        TransportOperation operation = NewOperationWithStop();

        operation.Cancel(" Client request ");

        Assert.Equal(TransportOperationStatus.Cancelled, operation.Status);
        Assert.Equal("Client request", operation.CancellationReason);
    }
}
