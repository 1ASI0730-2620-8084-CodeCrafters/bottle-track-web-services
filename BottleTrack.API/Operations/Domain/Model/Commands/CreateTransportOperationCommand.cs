namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class CreateTransportOperationCommand
{
    public Guid OrganizationId { get; }

    public string Description { get; }

    public DateOnly ScheduledDate { get; }

    public CreateTransportOperationCommand(Guid organizationId, string description, DateOnly scheduledDate)
    {
        OrganizationId = organizationId;
        Description = description;
        ScheduledDate = scheduledDate;
    }
}
