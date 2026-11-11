namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class DeliveryPointResource
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public Guid? OwnerUserId { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }
}
