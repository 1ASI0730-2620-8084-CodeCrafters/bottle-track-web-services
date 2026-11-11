using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Queries;

namespace BottleTrack.API.Operations.Application.Services;

public interface ITransportOperationQueryService
{
    Task<TransportOperation?> Handle(GetTransportOperationByIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TransportOperation>> Handle(GetTransportOperationsByOrganizationIdQuery query,
        CancellationToken cancellationToken = default);
}
