using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Iam.Interfaces.Rest.Resources;

public class UpdateOrganizationResource
{
    [Required]
    public string BusinessName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}
