using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class DispatchTransportOperationResource
{
    [Required]
    public Guid VehicleId { get; set; }

    [Required]
    public Guid DriverId { get; set; }
}
