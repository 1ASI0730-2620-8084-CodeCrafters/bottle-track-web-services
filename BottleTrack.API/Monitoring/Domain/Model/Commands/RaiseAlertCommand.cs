using BottleTrack.API.Monitoring.Domain.Model.ValueObjects;

namespace BottleTrack.API.Monitoring.Domain.Model.Commands;

public class RaiseAlertCommand
{
    public Guid IotDeviceId { get; }

    public Guid TransportOperationId { get; }

    public AlertType Type { get; }

    public RaiseAlertCommand(Guid iotDeviceId, Guid transportOperationId, AlertType type)
    {
        IotDeviceId = iotDeviceId;
        TransportOperationId = transportOperationId;
        Type = type;
    }
}
