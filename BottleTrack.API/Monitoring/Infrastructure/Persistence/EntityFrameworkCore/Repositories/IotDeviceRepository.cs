using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class IotDeviceRepository : BaseRepository<IotDevice>, IIotDeviceRepository
{
    public IotDeviceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IotDevice?> FindBySerialNumberAsync(string serialNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>()
            .FirstOrDefaultAsync(device => device.SerialNumber == serialNumber, cancellationToken);
    }
}
