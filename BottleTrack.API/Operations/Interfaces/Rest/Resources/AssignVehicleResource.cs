using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class AssignVehicleResource
{
    [Required]
    public Guid VehicleId { get; set; }
}
