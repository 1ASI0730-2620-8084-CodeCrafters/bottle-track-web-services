namespace BottleTrack.API.Incidents.Domain.Model.Commands;

public class AddEvidenceCommand
{
    public Guid IncidentId { get; }

    public string FileUrl { get; }

    public string FileType { get; }

    public Guid UploadedBy { get; }

    public AddEvidenceCommand(Guid incidentId, string fileUrl, string fileType, Guid uploadedBy)
    {
        IncidentId = incidentId;
        FileUrl = fileUrl;
        FileType = fileType;
        UploadedBy = uploadedBy;
    }
}
