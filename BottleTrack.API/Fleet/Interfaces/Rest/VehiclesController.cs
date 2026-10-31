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
[Tags("Vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleCommandService vehicleCommandService;
    private readonly IVehicleQueryService vehicleQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public VehiclesController(IVehicleCommandService vehicleCommandService, IVehicleQueryService vehicleQueryService,
        IStringLocalizer<SharedResource> localizer)
    {
        this.vehicleCommandService = vehicleCommandService;
        this.vehicleQueryService = vehicleQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterVehicle([FromBody] RegisterVehicleResource resource,
        CancellationToken cancellationToken)
    {
        var command = VehicleAssembler.ToCommand(resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            vehicle => CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id },
                VehicleAssembler.ToResource(vehicle)),
            error => ErrorResult(error));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetVehiclesByOrganizationId([FromQuery] Guid organizationId,
        [FromQuery] bool onlyAvailable, CancellationToken cancellationToken)
    {
        IEnumerable<Vehicle> vehicles = await vehicleQueryService.Handle(
            new GetVehiclesByOrganizationIdQuery(organizationId, onlyAvailable), cancellationToken);

        List<VehicleResource> resources = new List<VehicleResource>();
        foreach (Vehicle vehicle in vehicles)
            resources.Add(VehicleAssembler.ToResource(vehicle));

        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await vehicleQueryService.Handle(new GetVehicleByIdQuery(id), cancellationToken);

        if (vehicle == null)
            return Problem(detail: localizer["VehicleNotFound"].Value, statusCode: StatusCodes.Status404NotFound);

        return Ok(VehicleAssembler.ToResource(vehicle));
    }

    private ActionResult ErrorResult(VehicleError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == VehicleError.PlateNumberAlreadyRegistered)
            statusCode = StatusCodes.Status409Conflict;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
