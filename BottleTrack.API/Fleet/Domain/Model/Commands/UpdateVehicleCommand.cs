using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Domain.Model.Commands;

public class UpdateVehicleCommand
{
    public Guid VehicleId { get; }

    public string Brand { get; }

    public string Model { get; }

    public int CapacityInBoxes { get; }

    public VehicleStatus? Status { get; }

    public UpdateVehicleCommand(Guid vehicleId, string brand, string model, int capacityInBoxes, VehicleStatus? status)
    {
        VehicleId = vehicleId;
        Brand = brand;
        Model = model;
        CapacityInBoxes = capacityInBoxes;
        Status = status;
    }
}
