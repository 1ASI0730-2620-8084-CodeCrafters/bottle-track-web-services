using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class RegisterDeliveryPointResource
{
    [Required]
    public Guid OrganizationId { get; set; }

    public Guid? OwnerUserId { get; set; }

    [Required]
    public string BusinessName { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string District { get; set; } = string.Empty;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }
}
