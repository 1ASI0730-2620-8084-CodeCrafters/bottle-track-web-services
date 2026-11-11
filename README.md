# BottleTrack Web Services

RESTful API of **BottleTrack**, the bottled beverage transport tracking platform by **CodeCrafters**.

ASP.NET Core (.NET 10), Entity Framework Core and MySQL, organized as a modular monolith: one module per bounded context, each one with hexagonal layers.

## Run

1. Start MySQL. The API creates the `bottletrack` database and applies migrations on startup.
2. Check `BottleTrack.API/appsettings.Development.json` (user and password of your local MySQL).
3. Run the `https` profile. Swagger UI opens at `/swagger`.

## Structure

```
BottleTrack.API
├── Shared      cross-cutting base, owned by the whole team: change it only through a pull request
├── Iam         Identity and Access Management
├── Fleet       Fleet Management
├── Operations  Operations and Routes
├── Monitoring  IoT Monitoring
└── Incidents   Incident Management
```

Each bounded context folder has the same layers:

```
<Context>
├── Domain          Model (Aggregates, ValueObjects, Commands, Queries), Repositories
├── Application     Services (contracts), Internal (CommandServices, QueryServices), Errors
├── Infrastructure  Persistence/EntityFrameworkCore (Configuration, Repositories)
└── Interfaces      Rest (controllers, Resources, Transform)
```

## Adding a bounded context

1. Create `BottleTrack.API/<Context>/` with the four layers.
2. Primary keys are `Guid`. Repositories extend `BaseRepository<T>` and the interface extends `IBaseRepository<T>`.
3. Map your entities in one `IEntityTypeConfiguration<T>` class per aggregate inside `Infrastructure/Persistence/EntityFrameworkCore/Configuration`. `AppDbContext` discovers them automatically, so nobody edits it.
4. Register your repositories and services in `Program.cs`, below `IUnitOfWork`.
5. Controllers use `[Route("api/v1/[controller]")]`, plural names, and are converted to kebab-case.
6. Add new texts to the three `Resources/SharedResource*.resx` files.

## Conventions

GitFlow, Semantic Versioning, Conventional Commits, English identifiers and no code comments, as defined in section 5.1 of the project report.
