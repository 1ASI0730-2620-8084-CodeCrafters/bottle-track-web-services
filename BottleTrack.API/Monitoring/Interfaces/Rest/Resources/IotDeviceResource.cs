using BottleTrack.API.Monitoring.Domain.Model.ValueObjects;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class IotDeviceResource
{
    public Guid Id { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    public Guid? VehicleId { get; set; }

    public decimal SafeTemperatureMin { get; set; }

    public decimal SafeTemperatureMax { get; set; }

    public decimal ImpactThreshold { get; set; }

    public DeviceStatus Status { get; set; }
}
