namespace BottleTrack.API.Monitoring.Domain.Model.Queries;

public class GetSensorReadingsByDeviceIdQuery
{
    public Guid IotDeviceId { get; }

    public GetSensorReadingsByDeviceIdQuery(Guid iotDeviceId)
    {
        IotDeviceId = iotDeviceId;
    }
}
