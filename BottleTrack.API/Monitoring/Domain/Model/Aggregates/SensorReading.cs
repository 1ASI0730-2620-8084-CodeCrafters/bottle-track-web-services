namespace BottleTrack.API.Monitoring.Domain.Model.Aggregates;

public class SensorReading
{
    public Guid Id { get; private set; }

    public Guid IotDeviceId { get; private set; }

    public Guid? TransportOperationId { get; private set; }

    public DateTime RecordedAt { get; private set; }

    public decimal? Latitude { get; private set; }

    public decimal? Longitude { get; private set; }

    public decimal? TemperatureCelsius { get; private set; }

    public decimal? ImpactForce { get; private set; }

    private SensorReading()
    {
    }

    public static SensorReading Record(Guid iotDeviceId, Guid? transportOperationId, DateTime recordedAt,
        decimal? latitude, decimal? longitude, decimal? temperatureCelsius, decimal? impactForce)
    {
        SensorReading reading = new SensorReading();
        reading.Id = Guid.NewGuid();
        reading.IotDeviceId = iotDeviceId;
        reading.TransportOperationId = transportOperationId;
        reading.RecordedAt = recordedAt;
        reading.Latitude = latitude;
        reading.Longitude = longitude;
        reading.TemperatureCelsius = temperatureCelsius;
        reading.ImpactForce = impactForce;
        return reading;
    }
}
