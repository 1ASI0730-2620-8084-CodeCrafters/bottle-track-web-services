namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class CompleteTransportOperationCommand
{
    public Guid TransportOperationId { get; }

    public CompleteTransportOperationCommand(Guid transportOperationId)
    {
        TransportOperationId = transportOperationId;
    }
}
