using System.ComponentModel.DataAnnotations;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Interfaces.Rest.Resources;

public class AssignRoleResource
{
    [Required]
    public UserRole Role { get; set; }
}
