namespace BottleTrack.API.Operations.Domain.Model.Aggregates;

public class DeliveryPoint
{
    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public Guid? OwnerUserId { get; private set; }

    public string BusinessName { get; private set; }

    public string Address { get; private set; }

    public string District { get; private set; }

    public decimal? Latitude { get; private set; }

    public decimal? Longitude { get; private set; }

    private DeliveryPoint()
    {
        BusinessName = string.Empty;
        Address = string.Empty;
        District = string.Empty;
    }

    public static DeliveryPoint Register(Guid organizationId, Guid? ownerUserId, string businessName, string address,
        string district, decimal? latitude, decimal? longitude)
    {
        DeliveryPoint deliveryPoint = new DeliveryPoint();
        deliveryPoint.Id = Guid.NewGuid();
        deliveryPoint.OrganizationId = organizationId;
        deliveryPoint.OwnerUserId = ownerUserId;
        deliveryPoint.BusinessName = businessName.Trim();
        deliveryPoint.Address = address.Trim();
        deliveryPoint.District = district.Trim();
        deliveryPoint.Latitude = latitude;
        deliveryPoint.Longitude = longitude;
        return deliveryPoint;
    }
}
