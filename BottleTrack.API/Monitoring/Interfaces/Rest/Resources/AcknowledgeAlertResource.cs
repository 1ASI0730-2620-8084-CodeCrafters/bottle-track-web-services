using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class AcknowledgeAlertResource
{
    [Required]
    public Guid AcknowledgedBy { get; set; }
}
