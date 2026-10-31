using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Resources;

public class VehicleResource
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string PlateNumber { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int CapacityInBoxes { get; set; }

    public VehicleStatus Status { get; set; }
}
