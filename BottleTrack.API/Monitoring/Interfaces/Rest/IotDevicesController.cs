using System.Net.Mime;
using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
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
[Tags("IotDevices")]
public class IotDevicesController : ControllerBase
{
    private readonly IIotDeviceCommandService iotDeviceCommandService;
    private readonly IIotDeviceQueryService iotDeviceQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public IotDevicesController(IIotDeviceCommandService iotDeviceCommandService,
        IIotDeviceQueryService iotDeviceQueryService, IStringLocalizer<SharedResource> localizer)
    {
        this.iotDeviceCommandService = iotDeviceCommandService;
        this.iotDeviceQueryService = iotDeviceQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterIotDevice([FromBody] RegisterIotDeviceResource resource,
        CancellationToken cancellationToken)
    {
        var command = IotDeviceAssembler.ToCommand(resource);
        var result = await iotDeviceCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            device => CreatedAtAction(nameof(GetIotDeviceById), new { id = device.Id },
                IotDeviceAssembler.ToResource(device)),
            error => ErrorResult(error));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIotDeviceById(Guid id, CancellationToken cancellationToken)
    {
        IotDevice? device = await iotDeviceQueryService.Handle(new GetIotDeviceByIdQuery(id), cancellationToken);

        if (device == null)
            return ErrorResult(IotDeviceError.IotDeviceNotFound);

        return Ok(IotDeviceAssembler.ToResource(device));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IotDeviceResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllIotDevices(CancellationToken cancellationToken)
    {
        IEnumerable<IotDevice> devices = await iotDeviceQueryService.Handle(new GetAllIotDevicesQuery(),
            cancellationToken);

        List<IotDeviceResource> resources = new List<IotDeviceResource>();
        foreach (IotDevice device in devices)
            resources.Add(IotDeviceAssembler.ToResource(device));

        return Ok(resources);
    }

    private ActionResult ErrorResult(IotDeviceError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == IotDeviceError.IotDeviceNotFound)
            statusCode = StatusCodes.Status404NotFound;

        if (error == IotDeviceError.SerialNumberAlreadyRegistered)
            statusCode = StatusCodes.Status409Conflict;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
