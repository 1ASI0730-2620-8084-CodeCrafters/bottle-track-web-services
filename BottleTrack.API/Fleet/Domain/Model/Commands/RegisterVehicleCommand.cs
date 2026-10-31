namespace BottleTrack.API.Fleet.Domain.Model.Commands;

public class RegisterVehicleCommand
{
    public Guid OrganizationId { get; }

    public string PlateNumber { get; }

    public string Brand { get; }

    public string Model { get; }

    public int CapacityInBoxes { get; }

    public RegisterVehicleCommand(Guid organizationId, string plateNumber, string brand, string model,
        int capacityInBoxes)
    {
        OrganizationId = organizationId;
        PlateNumber = plateNumber;
        Brand = brand;
        Model = model;
        CapacityInBoxes = capacityInBoxes;
    }
}
