using BottleTrack.API.Monitoring.Domain.Model.ValueObjects;

namespace BottleTrack.API.Monitoring.Domain.Model.Aggregates;

public class Alert
{
    public Guid Id { get; private set; }

    public Guid IotDeviceId { get; private set; }

    public Guid TransportOperationId { get; private set; }

    public AlertType Type { get; private set; }

    public DateTime RaisedAt { get; private set; }

    public Guid? AcknowledgedBy { get; private set; }

    public DateTime? AcknowledgedAt { get; private set; }

    private Alert()
    {
    }

    public static Alert Raise(Guid iotDeviceId, Guid transportOperationId, AlertType type)
    {
        Alert alert = new Alert();
        alert.Id = Guid.NewGuid();
        alert.IotDeviceId = iotDeviceId;
        alert.TransportOperationId = transportOperationId;
        alert.Type = type;
        alert.RaisedAt = DateTime.UtcNow;
        return alert;
    }

    public void Acknowledge(Guid userId)
    {
        AcknowledgedBy = userId;
        AcknowledgedAt = DateTime.UtcNow;
    }
}
