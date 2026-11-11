using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    public AlertRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Alert>> ListByDeviceAsync(Guid iotDeviceId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Alert>()
            .Where(alert => alert.IotDeviceId == iotDeviceId)
            .ToListAsync(cancellationToken);
    }
}
