using BottleTrack.API.Shared.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;

    public UnitOfWork(AppDbContext context)
    {
        this.context = context;
    }

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
