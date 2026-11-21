namespace BottleTrack.API.Operations.Application.Errors;

public enum TransportOperationError
{
    InvalidTransportOperationData,
    TransportOperationNotFound,
    InvalidTransportOperationStatusChange,
    DeliveryStopNotFound,
    InvalidDeliveryResult,
    VehicleNotAvailable,
    DriverNotAvailable
}
