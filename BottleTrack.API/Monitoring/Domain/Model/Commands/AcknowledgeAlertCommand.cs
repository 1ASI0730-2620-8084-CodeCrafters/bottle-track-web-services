namespace BottleTrack.API.Monitoring.Domain.Model.Commands;

public class AcknowledgeAlertCommand
{
    public Guid AlertId { get; }

    public Guid AcknowledgedBy { get; }

    public AcknowledgeAlertCommand(Guid alertId, Guid acknowledgedBy)
    {
        AlertId = alertId;
        AcknowledgedBy = acknowledgedBy;
    }
}
