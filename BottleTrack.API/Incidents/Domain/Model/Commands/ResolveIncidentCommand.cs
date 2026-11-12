namespace BottleTrack.API.Incidents.Domain.Model.Commands;

public class ResolveIncidentCommand
{
    public Guid IncidentId { get; }

    public string Resolution { get; }

    public ResolveIncidentCommand(Guid incidentId, string resolution)
    {
        IncidentId = incidentId;
        Resolution = resolution;
    }
}
