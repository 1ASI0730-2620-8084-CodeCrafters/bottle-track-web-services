using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Model.Commands;
using BottleTrack.API.Incidents.Domain.Model.Entities;
using BottleTrack.API.Incidents.Interfaces.Rest.Resources;

namespace BottleTrack.API.Incidents.Interfaces.Rest.Transform;

public static class IncidentAssembler
{
    public static ReportIncidentCommand ToCommand(ReportIncidentResource resource)
    {
        return new ReportIncidentCommand(resource.TransportOperationId, resource.DeliveryStopId, resource.ReportedBy,
            resource.Source, resource.Type, resource.Severity, resource.Description, resource.AffectedQuantity);
    }

    public static IncidentResource ToResource(Incident incident)
    {
        IncidentResource resource = new IncidentResource();
        resource.Id = incident.Id;
        resource.Code = incident.Code;
        resource.TransportOperationId = incident.TransportOperationId;
        resource.DeliveryStopId = incident.DeliveryStopId;
        resource.ReportedBy = incident.ReportedBy;
        resource.Source = incident.Source;
        resource.Type = incident.Type;
        resource.Severity = incident.Severity;
        resource.Description = incident.Description;
        resource.AffectedQuantity = incident.AffectedQuantity;
        resource.Status = incident.Status;
        resource.Resolution = incident.Resolution;
        resource.ReportedAt = incident.ReportedAt;
        resource.ResolvedAt = incident.ResolvedAt;

        foreach (Evidence evidence in incident.Evidences)
        {
            EvidenceResource evidenceResource = new EvidenceResource();
            evidenceResource.Id = evidence.Id;
            evidenceResource.FileUrl = evidence.FileUrl;
            evidenceResource.FileType = evidence.FileType;
            evidenceResource.UploadedBy = evidence.UploadedBy;
            evidenceResource.UploadedAt = evidence.UploadedAt;
            resource.Evidences.Add(evidenceResource);
        }

        return resource;
    }
}
