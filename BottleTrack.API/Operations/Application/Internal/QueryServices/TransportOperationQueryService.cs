using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Queries;
using BottleTrack.API.Operations.Domain.Repositories;

namespace BottleTrack.API.Operations.Application.Internal.QueryServices;

public class TransportOperationQueryService : ITransportOperationQueryService
{
    private readonly ITransportOperationRepository transportOperationRepository;

    public TransportOperationQueryService(ITransportOperationRepository transportOperationRepository)
    {
        this.transportOperationRepository = transportOperationRepository;
    }

    public async Task<TransportOperation?> Handle(GetTransportOperationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await transportOperationRepository.FindByIdWithStopsAsync(query.TransportOperationId, cancellationToken);
    }

    public async Task<IEnumerable<TransportOperation>> Handle(GetTransportOperationsByOrganizationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await transportOperationRepository.ListByOrganizationAsync(query.OrganizationId, cancellationToken);
    }
}
