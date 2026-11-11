namespace BottleTrack.API.Incidents.Domain.Model.Entities;

public class Evidence
{
    public Guid Id { get; private set; }

    public Guid IncidentId { get; private set; }

    public string FileUrl { get; private set; }

    public string FileType { get; private set; }

    public Guid UploadedBy { get; private set; }

    public DateTime UploadedAt { get; private set; }

    private Evidence()
    {
        FileUrl = string.Empty;
        FileType = string.Empty;
    }

    public static Evidence Create(Guid incidentId, string fileUrl, string fileType, Guid uploadedBy)
    {
        Evidence evidence = new Evidence();
        evidence.Id = Guid.NewGuid();
        evidence.IncidentId = incidentId;
        evidence.FileUrl = fileUrl.Trim();
        evidence.FileType = fileType.Trim();
        evidence.UploadedBy = uploadedBy;
        evidence.UploadedAt = DateTime.UtcNow;
        return evidence;
    }
}
