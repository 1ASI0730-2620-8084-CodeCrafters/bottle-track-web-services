using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class AddDeliveryStopResource
{
    [Required]
    public Guid DeliveryPointId { get; set; }

    [Required]
    public int SequenceNumber { get; set; }

    [Required]
    public int PlannedQuantity { get; set; }
}
