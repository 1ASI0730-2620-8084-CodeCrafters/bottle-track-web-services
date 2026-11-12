using BottleTrack.API.Incidents.Domain.Model.Entities;
using BottleTrack.API.Incidents.Domain.Model.ValueObjects;

namespace BottleTrack.API.Incidents.Domain.Model.Aggregates;

public class Incident
{
    public Guid Id { get; private set; }

    public string Code { get; private set; }

    public Guid TransportOperationId { get; private set; }

    public Guid? DeliveryStopId { get; private set; }

    public Guid ReportedBy { get; private set; }

    public IncidentSource Source { get; private set; }

    public IncidentType Type { get; private set; }

    public IncidentSeverity Severity { get; private set; }

    public string Description { get; private set; }

    public int? AffectedQuantity { get; private set; }

    public IncidentStatus Status { get; private set; }

    public string? Resolution { get; private set; }

    public DateTime ReportedAt { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public List<Evidence> Evidences { get; private set; }

    private Incident()
    {
        Code = string.Empty;
        Description = string.Empty;
        Evidences = new List<Evidence>();
    }

    public static Incident Report(string code, Guid transportOperationId, Guid? deliveryStopId, Guid reportedBy,
        IncidentSource source, IncidentType type, IncidentSeverity severity, string description, int? affectedQuantity)
    {
        Incident incident = new Incident();
        incident.Id = Guid.NewGuid();
        incident.Code = code;
        incident.TransportOperationId = transportOperationId;
        incident.DeliveryStopId = deliveryStopId;
        incident.ReportedBy = reportedBy;
        incident.Source = source;
        incident.Type = type;
        incident.Severity = severity;
        incident.Description = description.Trim();
        incident.AffectedQuantity = affectedQuantity;
        incident.Status = IncidentStatus.Reported;
        incident.ReportedAt = DateTime.UtcNow;
        return incident;
    }

    public void AddEvidence(string fileUrl, string fileType, Guid uploadedBy)
    {
        Evidence evidence = Evidence.Create(Id, fileUrl, fileType, uploadedBy);
        Evidences.Add(evidence);
    }

    public void StartReview()
    {
        Status = IncidentStatus.UnderReview;
    }

    public void Resolve(string resolution)
    {
        Resolution = resolution.Trim();
        Status = IncidentStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        Status = IncidentStatus.Closed;
    }
}
