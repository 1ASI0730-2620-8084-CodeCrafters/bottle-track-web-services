using System.Net.Mime;
using BottleTrack.API.Fleet.Application.Errors;
using BottleTrack.API.Fleet.Application.Services;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Queries;
using BottleTrack.API.Fleet.Interfaces.Rest.Resources;
using BottleTrack.API.Fleet.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Fleet.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Drivers")]
public class DriversController : ControllerBase
{
    private readonly IDriverCommandService driverCommandService;
    private readonly IDriverQueryService driverQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public DriversController(IDriverCommandService driverCommandService, IDriverQueryService driverQueryService,
        IStringLocalizer<SharedResource> localizer)
    {
        this.driverCommandService = driverCommandService;
        this.driverQueryService = driverQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterDriver([FromBody] RegisterDriverResource resource,
        CancellationToken cancellationToken)
    {
        var command = DriverAssembler.ToCommand(resource);
        var result = await driverCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            driver => CreatedAtAction(nameof(GetDriverById), new { id = driver.Id },
                DriverAssembler.ToResource(driver)),
            error => ErrorResult(error));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DriverResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetDriversByOrganizationId([FromQuery] Guid organizationId,
        [FromQuery] bool onlyAvailable, CancellationToken cancellationToken)
    {
        IEnumerable<Driver> drivers = await driverQueryService.Handle(
            new GetDriversByOrganizationIdQuery(organizationId, onlyAvailable), cancellationToken);

        List<DriverResource> resources = new List<DriverResource>();
        foreach (Driver driver in drivers)
            resources.Add(DriverAssembler.ToResource(driver));

        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DriverResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDriverById(Guid id, CancellationToken cancellationToken)
    {
        Driver? driver = await driverQueryService.Handle(new GetDriverByIdQuery(id), cancellationToken);

        if (driver == null)
            return Problem(detail: localizer["DriverNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        return Ok(DriverAssembler.ToResource(driver));
    }

    private ActionResult ErrorResult(DriverError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == DriverError.DocumentNumberAlreadyRegistered || error == DriverError.LicenseNumberAlreadyRegistered)
            statusCode = StatusCodes.Status409Conflict;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
