namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class StartTransportOperationCommand
{
    public Guid TransportOperationId { get; }

    public StartTransportOperationCommand(Guid transportOperationId)
    {
        TransportOperationId = transportOperationId;
    }
}
