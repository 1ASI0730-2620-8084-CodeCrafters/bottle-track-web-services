using System.ComponentModel.DataAnnotations;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Interfaces.Rest.Resources;

public class RegisterUserResource
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }
}
