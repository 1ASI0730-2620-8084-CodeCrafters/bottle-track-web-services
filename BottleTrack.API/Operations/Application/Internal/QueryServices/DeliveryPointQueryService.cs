using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Queries;
using BottleTrack.API.Operations.Domain.Repositories;

namespace BottleTrack.API.Operations.Application.Internal.QueryServices;

public class DeliveryPointQueryService : IDeliveryPointQueryService
{
    private readonly IDeliveryPointRepository deliveryPointRepository;

    public DeliveryPointQueryService(IDeliveryPointRepository deliveryPointRepository)
    {
        this.deliveryPointRepository = deliveryPointRepository;
    }

    public async Task<DeliveryPoint?> Handle(GetDeliveryPointByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await deliveryPointRepository.FindByIdAsync(query.DeliveryPointId, cancellationToken);
    }

    public async Task<IEnumerable<DeliveryPoint>> Handle(GetDeliveryPointsByOrganizationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await deliveryPointRepository.ListByOrganizationAsync(query.OrganizationId, cancellationToken);
    }
}
