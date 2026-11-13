using System.Net.Mime;
using System.Text.Json;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Queries;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Interfaces.Rest.Resources;
using BottleTrack.API.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace BottleTrack.API.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Roles")]
public class RolesController : ControllerBase
{
    private readonly IUserQueryService userQueryService;

    public RolesController(IUserQueryService userQueryService)
    {
        this.userQueryService = userQueryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult GetRoles()
    {
        List<string> roles = new List<string>();
        foreach (UserRole role in Enum.GetValues<UserRole>())
            roles.Add(JsonNamingPolicy.SnakeCaseLower.ConvertName(role.ToString()));

        return Ok(roles);
    }

    [HttpGet("{role}/users")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUsersByRole(string role, [FromQuery] Guid organizationId,
        CancellationToken cancellationToken)
    {
        UserRole? userRole = FindRole(role);
        if (userRole == null)
            return NotFound();

        IEnumerable<User> users = await userQueryService.Handle(new GetUsersByRoleQuery(organizationId, userRole.Value),
            cancellationToken);

        List<UserResource> resources = new List<UserResource>();
        foreach (User user in users)
            resources.Add(UserAssembler.ToResource(user));

        return Ok(resources);
    }

    private UserRole? FindRole(string role)
    {
        foreach (UserRole candidate in Enum.GetValues<UserRole>())
        {
            if (JsonNamingPolicy.SnakeCaseLower.ConvertName(candidate.ToString()) == role)
                return candidate;
        }

        return null;
    }
}
