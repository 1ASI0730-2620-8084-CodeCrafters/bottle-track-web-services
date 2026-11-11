namespace BottleTrack.API.Incidents.Interfaces.Rest.Resources;

public class EvidenceResource
{
    public Guid Id { get; set; }

    public string FileUrl { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;

    public Guid UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }
}
