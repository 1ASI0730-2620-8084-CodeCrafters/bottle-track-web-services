using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Incidents.Interfaces.Rest.Resources;

public class AddEvidenceResource
{
    [Required]
    public string FileUrl { get; set; } = string.Empty;

    [Required]
    public string FileType { get; set; } = string.Empty;

    [Required]
    public Guid UploadedBy { get; set; }
}
