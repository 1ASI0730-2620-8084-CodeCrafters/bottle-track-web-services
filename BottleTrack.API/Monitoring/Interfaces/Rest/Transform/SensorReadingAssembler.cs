using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Transform;

public static class SensorReadingAssembler
{
    public static RecordSensorReadingCommand ToCommand(RecordSensorReadingResource resource)
    {
        return new RecordSensorReadingCommand(resource.IotDeviceId, resource.TransportOperationId, resource.RecordedAt,
            resource.Latitude, resource.Longitude, resource.TemperatureCelsius, resource.ImpactForce);
    }

    public static SensorReadingResource ToResource(SensorReading reading)
    {
        SensorReadingResource resource = new SensorReadingResource();
        resource.Id = reading.Id;
        resource.IotDeviceId = reading.IotDeviceId;
        resource.TransportOperationId = reading.TransportOperationId;
        resource.RecordedAt = reading.RecordedAt;
        resource.Latitude = reading.Latitude;
        resource.Longitude = reading.Longitude;
        resource.TemperatureCelsius = reading.TemperatureCelsius;
        resource.ImpactForce = reading.ImpactForce;
        return resource;
    }
}
