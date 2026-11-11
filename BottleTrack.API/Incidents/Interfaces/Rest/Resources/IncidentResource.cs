using BottleTrack.API.Incidents.Domain.Model.ValueObjects;

namespace BottleTrack.API.Incidents.Interfaces.Rest.Resources;

public class IncidentResource
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public Guid TransportOperationId { get; set; }

    public Guid? DeliveryStopId { get; set; }

    public Guid ReportedBy { get; set; }

    public IncidentSource Source { get; set; }

    public IncidentType Type { get; set; }

    public IncidentSeverity Severity { get; set; }

    public string Description { get; set; } = string.Empty;

    public int? AffectedQuantity { get; set; }

    public IncidentStatus Status { get; set; }

    public string? Resolution { get; set; }

    public DateTime ReportedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public List<EvidenceResource> Evidences { get; set; } = new List<EvidenceResource>();
}
