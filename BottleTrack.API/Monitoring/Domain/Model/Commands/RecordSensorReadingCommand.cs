namespace BottleTrack.API.Monitoring.Domain.Model.Commands;

public class RecordSensorReadingCommand
{
    public Guid IotDeviceId { get; }

    public Guid? TransportOperationId { get; }

    public DateTime RecordedAt { get; }

    public decimal? Latitude { get; }

    public decimal? Longitude { get; }

    public decimal? TemperatureCelsius { get; }

    public decimal? ImpactForce { get; }

    public RecordSensorReadingCommand(Guid iotDeviceId, Guid? transportOperationId, DateTime recordedAt,
        decimal? latitude, decimal? longitude, decimal? temperatureCelsius, decimal? impactForce)
    {
        IotDeviceId = iotDeviceId;
        TransportOperationId = transportOperationId;
        RecordedAt = recordedAt;
        Latitude = latitude;
        Longitude = longitude;
        TemperatureCelsius = temperatureCelsius;
        ImpactForce = impactForce;
    }
}
