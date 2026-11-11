using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Resources;

public class UpdateDriverResource
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}
