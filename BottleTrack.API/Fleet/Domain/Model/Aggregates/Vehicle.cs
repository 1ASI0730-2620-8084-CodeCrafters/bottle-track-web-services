using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Domain.Model.Aggregates;

public class Vehicle
{
    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public PlateNumber PlateNumber { get; private set; }

    public string Brand { get; private set; }

    public string Model { get; private set; }

    public int CapacityInBoxes { get; private set; }

    public VehicleStatus Status { get; private set; }

    private Vehicle()
    {
        PlateNumber = null!;
        Brand = string.Empty;
        Model = string.Empty;
    }

    public static Vehicle Register(Guid organizationId, PlateNumber plateNumber, string brand, string model,
        int capacityInBoxes)
    {
        Vehicle vehicle = new Vehicle();
        vehicle.Id = Guid.NewGuid();
        vehicle.OrganizationId = organizationId;
        vehicle.PlateNumber = plateNumber;
        vehicle.Brand = brand.Trim();
        vehicle.Model = model.Trim();
        vehicle.CapacityInBoxes = capacityInBoxes;
        vehicle.Status = VehicleStatus.Available;
        return vehicle;
    }

    public void UpdateDetails(string brand, string model, int capacityInBoxes)
    {
        Brand = brand.Trim();
        Model = model.Trim();
        CapacityInBoxes = capacityInBoxes;
    }

    public void Reserve()
    {
        if (Status != VehicleStatus.Available)
            throw new InvalidOperationException("Only an available vehicle can be reserved.");

        Status = VehicleStatus.Assigned;
    }

    public void Release()
    {
        if (Status != VehicleStatus.Assigned)
            throw new InvalidOperationException("Only an assigned vehicle can be released.");

        Status = VehicleStatus.Available;
    }

    public void SendToMaintenance()
    {
        if (Status == VehicleStatus.Assigned)
            throw new InvalidOperationException("An assigned vehicle cannot be sent to maintenance.");

        Status = VehicleStatus.InMaintenance;
    }
}
