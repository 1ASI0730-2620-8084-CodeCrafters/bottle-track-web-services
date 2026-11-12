using BottleTrack.API.Incidents.Domain.Model.ValueObjects;

namespace BottleTrack.API.Incidents.Domain.Model.Commands;

public class ReportIncidentCommand
{
    public Guid TransportOperationId { get; }

    public Guid? DeliveryStopId { get; }

    public Guid ReportedBy { get; }

    public IncidentSource Source { get; }

    public IncidentType Type { get; }

    public IncidentSeverity Severity { get; }

    public string Description { get; }

    public int? AffectedQuantity { get; }

    public ReportIncidentCommand(Guid transportOperationId, Guid? deliveryStopId, Guid reportedBy,
        IncidentSource source, IncidentType type, IncidentSeverity severity, string description, int? affectedQuantity)
    {
        TransportOperationId = transportOperationId;
        DeliveryStopId = deliveryStopId;
        ReportedBy = reportedBy;
        Source = source;
        Type = type;
        Severity = severity;
        Description = description;
        AffectedQuantity = affectedQuantity;
    }
}
