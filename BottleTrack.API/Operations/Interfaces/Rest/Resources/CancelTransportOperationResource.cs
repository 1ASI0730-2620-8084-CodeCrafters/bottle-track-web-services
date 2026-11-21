using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class CancelTransportOperationResource
{
    [Required]
    public string Reason { get; set; } = string.Empty;
}
