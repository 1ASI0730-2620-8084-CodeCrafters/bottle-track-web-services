using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Incidents.Interfaces.Rest.Resources;

public class ResolveIncidentResource
{
    [Required]
    public string Resolution { get; set; } = string.Empty;
}
