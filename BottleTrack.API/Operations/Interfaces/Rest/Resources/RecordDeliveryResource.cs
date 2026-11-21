using System.ComponentModel.DataAnnotations;
using BottleTrack.API.Operations.Domain.Model.ValueObjects;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class RecordDeliveryResource
{
    [Required]
    public DeliveryStopStatus Status { get; set; }

    public int DeliveredQuantity { get; set; }

    public string? FailureReason { get; set; }
}
