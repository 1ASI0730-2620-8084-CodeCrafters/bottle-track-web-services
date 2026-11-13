using System.Net.Mime;
using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Domain.Model.Queries;
using BottleTrack.API.Monitoring.Interfaces.Rest.Resources;
using BottleTrack.API.Monitoring.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Alerts")]
public class AlertsController : ControllerBase
{
    private readonly IAlertCommandService alertCommandService;
    private readonly IAlertQueryService alertQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public AlertsController(IAlertCommandService alertCommandService, IAlertQueryService alertQueryService,
        IStringLocalizer<SharedResource> localizer)
    {
        this.alertCommandService = alertCommandService;
        this.alertQueryService = alertQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RaiseAlert([FromBody] RaiseAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = AlertAssembler.ToCommand(resource);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            alert => CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, AlertAssembler.ToResource(alert)),
            error => ErrorResult(error));
    }

    [HttpPost("{id:guid}/acknowledgments")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AcknowledgeAlert(Guid id, [FromBody] AcknowledgeAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AcknowledgeAlertCommand(id, resource.AcknowledgedBy);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            alert => Ok(AlertAssembler.ToResource(alert)),
            error => ErrorResult(error));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAlertById(Guid id, CancellationToken cancellationToken)
    {
        Alert? alert = await alertQueryService.Handle(new GetAlertByIdQuery(id), cancellationToken);

        if (alert == null)
            return ErrorResult(AlertError.AlertNotFound);

        return Ok(AlertAssembler.ToResource(alert));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAlertsByDeviceId([FromQuery] Guid iotDeviceId,
        CancellationToken cancellationToken)
    {
        IEnumerable<Alert> alerts = await alertQueryService.Handle(new GetAlertsByDeviceIdQuery(iotDeviceId),
            cancellationToken);

        List<AlertResource> resources = new List<AlertResource>();
        foreach (Alert alert in alerts)
            resources.Add(AlertAssembler.ToResource(alert));

        return Ok(resources);
    }

    private ActionResult ErrorResult(AlertError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == AlertError.AlertNotFound)
            statusCode = StatusCodes.Status404NotFound;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
