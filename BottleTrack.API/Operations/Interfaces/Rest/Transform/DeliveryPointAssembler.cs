using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Operations.Interfaces.Rest.Resources;

namespace BottleTrack.API.Operations.Interfaces.Rest.Transform;

public static class DeliveryPointAssembler
{
    public static RegisterDeliveryPointCommand ToCommand(RegisterDeliveryPointResource resource)
    {
        return new RegisterDeliveryPointCommand(resource.OrganizationId, resource.OwnerUserId, resource.BusinessName,
            resource.Address, resource.District, resource.Latitude, resource.Longitude);
    }

    public static DeliveryPointResource ToResource(DeliveryPoint deliveryPoint)
    {
        DeliveryPointResource resource = new DeliveryPointResource();
        resource.Id = deliveryPoint.Id;
        resource.OrganizationId = deliveryPoint.OrganizationId;
        resource.OwnerUserId = deliveryPoint.OwnerUserId;
        resource.BusinessName = deliveryPoint.BusinessName;
        resource.Address = deliveryPoint.Address;
        resource.District = deliveryPoint.District;
        resource.Latitude = deliveryPoint.Latitude;
        resource.Longitude = deliveryPoint.Longitude;
        return resource;
    }
}
