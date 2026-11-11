using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Queries;

namespace BottleTrack.API.Operations.Application.Services;

public interface IDeliveryPointQueryService
{
    Task<DeliveryPoint?> Handle(GetDeliveryPointByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<DeliveryPoint>> Handle(GetDeliveryPointsByOrganizationIdQuery query,
        CancellationToken cancellationToken = default);
}
