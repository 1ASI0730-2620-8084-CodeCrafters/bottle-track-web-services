using System.Net.Mime;
using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Queries;
using BottleTrack.API.Iam.Interfaces.Rest.Resources;
using BottleTrack.API.Iam.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationCommandService organizationCommandService;
    private readonly IOrganizationQueryService organizationQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public OrganizationsController(IOrganizationCommandService organizationCommandService,
        IOrganizationQueryService organizationQueryService, IStringLocalizer<SharedResource> localizer)
    {
        this.organizationCommandService = organizationCommandService;
        this.organizationQueryService = organizationQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrganizationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterOrganization([FromBody] RegisterOrganizationResource resource,
        CancellationToken cancellationToken)
    {
        var command = OrganizationAssembler.ToCommand(resource);
        var result = await organizationCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            organization => CreatedAtAction(nameof(GetOrganizationById), new { id = organization.Id },
                OrganizationAssembler.ToResource(organization)),
            error => ErrorResult(error));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrganizationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetOrganizationById(Guid id, CancellationToken cancellationToken)
    {
        Organization? organization = await organizationQueryService.Handle(new GetOrganizationByIdQuery(id),
            cancellationToken);

        if (organization == null)
            return ErrorResult(OrganizationError.OrganizationNotFound);

        return Ok(OrganizationAssembler.ToResource(organization));
    }

    private ActionResult ErrorResult(OrganizationError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == OrganizationError.OrganizationNotFound)
            statusCode = StatusCodes.Status404NotFound;

        if (error == OrganizationError.TaxIdAlreadyRegistered)
            statusCode = StatusCodes.Status409Conflict;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
