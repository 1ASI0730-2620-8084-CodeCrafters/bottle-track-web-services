using BottleTrack.API.Monitoring.Domain.Model.ValueObjects;

namespace BottleTrack.API.Monitoring.Domain.Model.Aggregates;

public class IotDevice
{
    public Guid Id { get; private set; }

    public string SerialNumber { get; private set; }

    public Guid? VehicleId { get; private set; }

    public decimal SafeTemperatureMin { get; private set; }

    public decimal SafeTemperatureMax { get; private set; }

    public decimal ImpactThreshold { get; private set; }

    public DeviceStatus Status { get; private set; }

    private IotDevice()
    {
        SerialNumber = string.Empty;
    }

    public static IotDevice Register(string serialNumber, Guid? vehicleId, decimal safeTemperatureMin,
        decimal safeTemperatureMax, decimal impactThreshold)
    {
        IotDevice device = new IotDevice();
        device.Id = Guid.NewGuid();
        device.SerialNumber = serialNumber.Trim().ToUpperInvariant();
        device.VehicleId = vehicleId;
        device.SafeTemperatureMin = safeTemperatureMin;
        device.SafeTemperatureMax = safeTemperatureMax;
        device.ImpactThreshold = impactThreshold;
        device.Status = DeviceStatus.Active;
        return device;
    }

    public void GoOffline()
    {
        Status = DeviceStatus.Offline;
    }

    public void Retire()
    {
        Status = DeviceStatus.Retired;
    }
}
