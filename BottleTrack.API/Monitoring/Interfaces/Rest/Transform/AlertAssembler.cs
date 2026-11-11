using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Transform;

public static class AlertAssembler
{
    public static RaiseAlertCommand ToCommand(RaiseAlertResource resource)
    {
        return new RaiseAlertCommand(resource.IotDeviceId, resource.TransportOperationId, resource.Type);
    }

    public static AlertResource ToResource(Alert alert)
    {
        AlertResource resource = new AlertResource();
        resource.Id = alert.Id;
        resource.IotDeviceId = alert.IotDeviceId;
        resource.TransportOperationId = alert.TransportOperationId;
        resource.Type = alert.Type;
        resource.RaisedAt = alert.RaisedAt;
        resource.AcknowledgedBy = alert.AcknowledgedBy;
        resource.AcknowledgedAt = alert.AcknowledgedAt;
        return resource;
    }
}
