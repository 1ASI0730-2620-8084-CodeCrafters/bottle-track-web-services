using System.Net.Mime;
using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
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
[Tags("Users")]
public class UsersController : ControllerBase
{
    private readonly IUserCommandService userCommandService;
    private readonly IUserQueryService userQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public UsersController(IUserCommandService userCommandService, IUserQueryService userQueryService,
        IStringLocalizer<SharedResource> localizer)
    {
        this.userCommandService = userCommandService;
        this.userQueryService = userQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserResource resource,
        CancellationToken cancellationToken)
    {
        var command = UserAssembler.ToCommand(resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            user => CreatedAtAction(nameof(GetUserById), new { id = user.Id }, UserAssembler.ToResource(user)),
            error => ErrorResult(error));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUsersByOrganizationId([FromQuery] Guid organizationId,
        CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await userQueryService.Handle(new GetUsersByOrganizationIdQuery(organizationId),
            cancellationToken);

        List<UserResource> resources = new List<UserResource>();
        foreach (User user in users)
            resources.Add(UserAssembler.ToResource(user));

        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        User? user = await userQueryService.Handle(new GetUserByIdQuery(id), cancellationToken);

        if (user == null)
            return ErrorResult(UserError.UserNotFound);

        return Ok(UserAssembler.ToResource(user));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserResource resource,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, resource.FirstName, resource.LastName, resource.Email);
        var result = await userCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            user => Ok(UserAssembler.ToResource(user)),
            error => ErrorResult(error));
    }

    [HttpPut("{id:guid}/role")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AssignUserRole(Guid id, [FromBody] AssignRoleResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AssignUserRoleCommand(id, resource.Role);
        var result = await userCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            user => Ok(UserAssembler.ToResource(user)),
            error => ErrorResult(error));
    }

    private ActionResult ErrorResult(UserError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == UserError.UserNotFound || error == UserError.OrganizationNotFound)
            statusCode = StatusCodes.Status404NotFound;

        if (error == UserError.EmailAlreadyRegistered)
            statusCode = StatusCodes.Status409Conflict;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
