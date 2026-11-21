using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class AssignDriverResource
{
    [Required]
    public Guid DriverId { get; set; }
}
