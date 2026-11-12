namespace BottleTrack.API.Monitoring.Domain.Model.Queries;

public class GetAlertByIdQuery
{
    public Guid AlertId { get; }

    public GetAlertByIdQuery(Guid alertId)
    {
        AlertId = alertId;
    }
}
