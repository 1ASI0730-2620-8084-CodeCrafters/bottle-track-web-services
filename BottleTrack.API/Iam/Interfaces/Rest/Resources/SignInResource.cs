using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Iam.Interfaces.Rest.Resources;

public class SignInResource
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
