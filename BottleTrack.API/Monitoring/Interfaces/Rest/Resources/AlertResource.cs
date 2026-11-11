using BottleTrack.API.Monitoring.Domain.Model.ValueObjects;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class AlertResource
{
    public Guid Id { get; set; }

    public Guid IotDeviceId { get; set; }

    public Guid TransportOperationId { get; set; }

    public AlertType Type { get; set; }

    public DateTime RaisedAt { get; set; }

    public Guid? AcknowledgedBy { get; set; }

    public DateTime? AcknowledgedAt { get; set; }
}
