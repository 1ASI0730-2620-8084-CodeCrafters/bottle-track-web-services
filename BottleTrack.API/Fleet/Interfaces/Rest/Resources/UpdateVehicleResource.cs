using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Resources;

public class UpdateVehicleResource
{
    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    public int CapacityInBoxes { get; set; }
}
