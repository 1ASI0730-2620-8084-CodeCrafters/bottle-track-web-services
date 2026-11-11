namespace BottleTrack.API.Monitoring.Domain.Model.Queries;

public class GetIotDeviceByIdQuery
{
    public Guid IotDeviceId { get; }

    public GetIotDeviceByIdQuery(Guid iotDeviceId)
    {
        IotDeviceId = iotDeviceId;
    }
}
