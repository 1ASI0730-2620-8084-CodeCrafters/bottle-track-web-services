namespace BottleTrack.API.Monitoring.Domain.Model.Queries;

public class GetAlertsByDeviceIdQuery
{
    public Guid IotDeviceId { get; }

    public GetAlertsByDeviceIdQuery(Guid iotDeviceId)
    {
        IotDeviceId = iotDeviceId;
    }
}
