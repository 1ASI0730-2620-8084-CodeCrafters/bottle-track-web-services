using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class CreateTransportOperationResource
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateOnly ScheduledDate { get; set; }
}
