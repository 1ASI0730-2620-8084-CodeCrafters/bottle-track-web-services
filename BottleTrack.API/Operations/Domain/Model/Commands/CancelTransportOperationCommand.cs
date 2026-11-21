namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class CancelTransportOperationCommand
{
    public Guid TransportOperationId { get; }

    public string Reason { get; }

    public CancelTransportOperationCommand(Guid transportOperationId, string reason)
    {
        TransportOperationId = transportOperationId;
        Reason = reason;
    }
}
