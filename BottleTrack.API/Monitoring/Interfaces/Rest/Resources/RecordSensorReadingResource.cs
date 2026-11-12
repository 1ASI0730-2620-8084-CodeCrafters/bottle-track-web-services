using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class RecordSensorReadingResource
{
    [Required]
    public Guid IotDeviceId { get; set; }

    public Guid? TransportOperationId { get; set; }

    [Required]
    public DateTime RecordedAt { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? TemperatureCelsius { get; set; }

    public decimal? ImpactForce { get; set; }
}
