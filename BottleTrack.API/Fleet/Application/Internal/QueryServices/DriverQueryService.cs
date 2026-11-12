using BottleTrack.API.Fleet.Application.Services;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Queries;
using BottleTrack.API.Fleet.Domain.Repositories;

namespace BottleTrack.API.Fleet.Application.Internal.QueryServices;

public class DriverQueryService : IDriverQueryService
{
    private readonly IDriverRepository driverRepository;

    public DriverQueryService(IDriverRepository driverRepository)
    {
        this.driverRepository = driverRepository;
    }

    public async Task<Driver?> Handle(GetDriverByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await driverRepository.FindByIdAsync(query.DriverId, cancellationToken);
    }

    public async Task<IEnumerable<Driver>> Handle(GetDriversByOrganizationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.OnlyAvailable)
            return await driverRepository.ListAvailableAsync(query.OrganizationId, cancellationToken);

        return await driverRepository.ListByOrganizationAsync(query.OrganizationId, cancellationToken);
    }
}
