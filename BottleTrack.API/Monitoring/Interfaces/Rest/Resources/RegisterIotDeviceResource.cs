using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class RegisterIotDeviceResource
{
    [Required]
    public string SerialNumber { get; set; } = string.Empty;

    public Guid? VehicleId { get; set; }

    [Required]
    public decimal SafeTemperatureMin { get; set; }

    [Required]
    public decimal SafeTemperatureMax { get; set; }

    [Required]
    public decimal ImpactThreshold { get; set; }
}
