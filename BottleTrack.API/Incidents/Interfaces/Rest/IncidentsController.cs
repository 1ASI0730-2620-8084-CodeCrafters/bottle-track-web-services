using System.Net.Mime;
using BottleTrack.API.Incidents.Application.Errors;
using BottleTrack.API.Incidents.Application.Services;
using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Model.Commands;
using BottleTrack.API.Incidents.Domain.Model.Queries;
using BottleTrack.API.Incidents.Interfaces.Rest.Resources;
using BottleTrack.API.Incidents.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Incidents.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Incidents")]
public class IncidentsController : ControllerBase
{
    private readonly IIncidentCommandService incidentCommandService;
    private readonly IIncidentQueryService incidentQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public IncidentsController(IIncidentCommandService incidentCommandService,
        IIncidentQueryService incidentQueryService, IStringLocalizer<SharedResource> localizer)
    {
        this.incidentCommandService = incidentCommandService;
        this.incidentQueryService = incidentQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReportIncident([FromBody] ReportIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var command = IncidentAssembler.ToCommand(resource);
        var result = await incidentCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            incident => CreatedAtAction(nameof(GetIncidentById), new { id = incident.Id },
                IncidentAssembler.ToResource(incident)),
            error => ErrorResult(error));
    }

    [HttpPost("{id:guid}/evidences")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddEvidence(Guid id, [FromBody] AddEvidenceResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AddEvidenceCommand(id, resource.FileUrl, resource.FileType, resource.UploadedBy);
        var result = await incidentCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            incident => Ok(IncidentAssembler.ToResource(incident)),
            error => ErrorResult(error));
    }

    [HttpPost("{id:guid}/resolutions")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ResolveIncident(Guid id, [FromBody] ResolveIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var command = new ResolveIncidentCommand(id, resource.Resolution);
        var result = await incidentCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            incident => Ok(IncidentAssembler.ToResource(incident)),
            error => ErrorResult(error));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIncidentById(Guid id, CancellationToken cancellationToken)
    {
        Incident? incident = await incidentQueryService.Handle(new GetIncidentByIdQuery(id), cancellationToken);

        if (incident == null)
            return ErrorResult(IncidentError.IncidentNotFound);

        return Ok(IncidentAssembler.ToResource(incident));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IncidentResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetIncidentsByTransportOperationId([FromQuery] Guid transportOperationId,
        CancellationToken cancellationToken)
    {
        IEnumerable<Incident> incidents = await incidentQueryService.Handle(
            new GetIncidentsByTransportOperationIdQuery(transportOperationId), cancellationToken);

        List<IncidentResource> resources = new List<IncidentResource>();
        foreach (Incident incident in incidents)
            resources.Add(IncidentAssembler.ToResource(incident));

        return Ok(resources);
    }

    private ActionResult ErrorResult(IncidentError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == IncidentError.IncidentNotFound)
            statusCode = StatusCodes.Status404NotFound;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
