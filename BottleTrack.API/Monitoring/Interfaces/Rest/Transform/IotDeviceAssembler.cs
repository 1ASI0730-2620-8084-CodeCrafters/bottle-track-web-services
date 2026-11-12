using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Transform;

public static class IotDeviceAssembler
{
    public static RegisterIotDeviceCommand ToCommand(RegisterIotDeviceResource resource)
    {
        return new RegisterIotDeviceCommand(resource.SerialNumber, resource.VehicleId, resource.SafeTemperatureMin,
            resource.SafeTemperatureMax, resource.ImpactThreshold);
    }

    public static IotDeviceResource ToResource(IotDevice device)
    {
        IotDeviceResource resource = new IotDeviceResource();
        resource.Id = device.Id;
        resource.SerialNumber = device.SerialNumber;
        resource.VehicleId = device.VehicleId;
        resource.SafeTemperatureMin = device.SafeTemperatureMin;
        resource.SafeTemperatureMax = device.SafeTemperatureMax;
        resource.ImpactThreshold = device.ImpactThreshold;
        resource.Status = device.Status;
        return resource;
    }
}
