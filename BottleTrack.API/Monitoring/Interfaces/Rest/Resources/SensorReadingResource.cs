namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class SensorReadingResource
{
    public Guid Id { get; set; }

    public Guid IotDeviceId { get; set; }

    public Guid? TransportOperationId { get; set; }

    public DateTime RecordedAt { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? TemperatureCelsius { get; set; }

    public decimal? ImpactForce { get; set; }
}
