using System.ComponentModel.DataAnnotations;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Resources;

public class UpdateVehicleResource
{
    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    public int CapacityInBoxes { get; set; }

    public VehicleStatus? Status { get; set; }
}
