using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Commands;
using BottleTrack.API.Fleet.Interfaces.Rest.Resources;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Transform;

public static class VehicleAssembler
{
    public static RegisterVehicleCommand ToCommand(RegisterVehicleResource resource)
    {
        return new RegisterVehicleCommand(resource.OrganizationId, resource.PlateNumber, resource.Brand,
            resource.Model, resource.CapacityInBoxes);
    }

    public static VehicleResource ToResource(Vehicle vehicle)
    {
        VehicleResource resource = new VehicleResource();
        resource.Id = vehicle.Id;
        resource.OrganizationId = vehicle.OrganizationId;
        resource.PlateNumber = vehicle.PlateNumber.Value;
        resource.Brand = vehicle.Brand;
        resource.Model = vehicle.Model;
        resource.CapacityInBoxes = vehicle.CapacityInBoxes;
        resource.Status = vehicle.Status;
        return resource;
    }
}
