using System.Net.Mime;
using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Queries;
using BottleTrack.API.Operations.Interfaces.Rest.Resources;
using BottleTrack.API.Operations.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Operations.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("DeliveryPoints")]
public class DeliveryPointsController : ControllerBase
{
    private readonly IDeliveryPointCommandService deliveryPointCommandService;
    private readonly IDeliveryPointQueryService deliveryPointQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public DeliveryPointsController(IDeliveryPointCommandService deliveryPointCommandService,
        IDeliveryPointQueryService deliveryPointQueryService, IStringLocalizer<SharedResource> localizer)
    {
        this.deliveryPointCommandService = deliveryPointCommandService;
        this.deliveryPointQueryService = deliveryPointQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeliveryPointResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterDeliveryPoint([FromBody] RegisterDeliveryPointResource resource,
        CancellationToken cancellationToken)
    {
        var command = DeliveryPointAssembler.ToCommand(resource);
        var result = await deliveryPointCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            deliveryPoint => CreatedAtAction(nameof(GetDeliveryPointById), new { id = deliveryPoint.Id },
                DeliveryPointAssembler.ToResource(deliveryPoint)),
            error => ErrorResult(error));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DeliveryPointResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDeliveryPointById(Guid id, CancellationToken cancellationToken)
    {
        DeliveryPoint? deliveryPoint = await deliveryPointQueryService.Handle(new GetDeliveryPointByIdQuery(id),
            cancellationToken);

        if (deliveryPoint == null)
            return ErrorResult(DeliveryPointError.DeliveryPointNotFound);

        return Ok(DeliveryPointAssembler.ToResource(deliveryPoint));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DeliveryPointResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetDeliveryPointsByOrganizationId([FromQuery] Guid organizationId,
        CancellationToken cancellationToken)
    {
        IEnumerable<DeliveryPoint> deliveryPoints = await deliveryPointQueryService.Handle(
            new GetDeliveryPointsByOrganizationIdQuery(organizationId), cancellationToken);

        List<DeliveryPointResource> resources = new List<DeliveryPointResource>();
        foreach (DeliveryPoint deliveryPoint in deliveryPoints)
            resources.Add(DeliveryPointAssembler.ToResource(deliveryPoint));

        return Ok(resources);
    }

    private ActionResult ErrorResult(DeliveryPointError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == DeliveryPointError.DeliveryPointNotFound)
            statusCode = StatusCodes.Status404NotFound;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
