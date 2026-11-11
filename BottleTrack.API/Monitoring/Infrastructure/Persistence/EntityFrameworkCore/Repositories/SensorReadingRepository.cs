using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SensorReadingRepository : BaseRepository<SensorReading>, ISensorReadingRepository
{
    public SensorReadingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SensorReading>> ListByDeviceAsync(Guid iotDeviceId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<SensorReading>()
            .Where(reading => reading.IotDeviceId == iotDeviceId)
            .ToListAsync(cancellationToken);
    }
}
