using System.Net.Mime;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Interfaces.Rest.Resources;
using BottleTrack.API.Iam.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationCommandService authenticationCommandService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public AuthenticationController(IAuthenticationCommandService authenticationCommandService,
        IStringLocalizer<SharedResource> localizer)
    {
        this.authenticationCommandService = authenticationCommandService;
        this.localizer = localizer;
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> SignIn([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        var command = AuthenticationAssembler.ToCommand(resource);
        var result = await authenticationCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            authenticated => Ok(AuthenticationAssembler.ToResource(authenticated)),
            error => Problem(detail: localizer[error.ToString()].Value,
                statusCode: StatusCodes.Status401Unauthorized));
    }
}
