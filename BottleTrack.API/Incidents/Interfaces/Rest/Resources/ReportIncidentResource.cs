using System.ComponentModel.DataAnnotations;
using BottleTrack.API.Incidents.Domain.Model.ValueObjects;

namespace BottleTrack.API.Incidents.Interfaces.Rest.Resources;

public class ReportIncidentResource
{
    [Required]
    public Guid TransportOperationId { get; set; }

    public Guid? DeliveryStopId { get; set; }

    [Required]
    public Guid ReportedBy { get; set; }

    [Required]
    public IncidentSource Source { get; set; }

    [Required]
    public IncidentType Type { get; set; }

    [Required]
    public IncidentSeverity Severity { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public int? AffectedQuantity { get; set; }
}
