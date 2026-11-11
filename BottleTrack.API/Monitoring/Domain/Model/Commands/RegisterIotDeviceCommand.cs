namespace BottleTrack.API.Monitoring.Domain.Model.Commands;

public class RegisterIotDeviceCommand
{
    public string SerialNumber { get; }

    public Guid? VehicleId { get; }

    public decimal SafeTemperatureMin { get; }

    public decimal SafeTemperatureMax { get; }

    public decimal ImpactThreshold { get; }

    public RegisterIotDeviceCommand(string serialNumber, Guid? vehicleId, decimal safeTemperatureMin,
        decimal safeTemperatureMax, decimal impactThreshold)
    {
        SerialNumber = serialNumber;
        VehicleId = vehicleId;
        SafeTemperatureMin = safeTemperatureMin;
        SafeTemperatureMax = safeTemperatureMax;
        ImpactThreshold = impactThreshold;
    }
}
