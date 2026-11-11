using System.Net.Mime;
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
[Tags("SensorReadings")]
public class SensorReadingsController : ControllerBase
{
    private readonly ISensorReadingCommandService sensorReadingCommandService;
    private readonly ISensorReadingQueryService sensorReadingQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public SensorReadingsController(ISensorReadingCommandService sensorReadingCommandService,
        ISensorReadingQueryService sensorReadingQueryService, IStringLocalizer<SharedResource> localizer)
    {
        this.sensorReadingCommandService = sensorReadingCommandService;
        this.sensorReadingQueryService = sensorReadingQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SensorReadingResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RecordSensorReading([FromBody] RecordSensorReadingResource resource,
        CancellationToken cancellationToken)
    {
        var command = SensorReadingAssembler.ToCommand(resource);
        var result = await sensorReadingCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            reading => CreatedAtAction(nameof(GetSensorReadingsByDeviceId), new { iotDeviceId = reading.IotDeviceId },
                SensorReadingAssembler.ToResource(reading)),
            error => Problem(detail: localizer[error.ToString()].Value, statusCode: StatusCodes.Status400BadRequest));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SensorReadingResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetSensorReadingsByDeviceId([FromQuery] Guid iotDeviceId,
        CancellationToken cancellationToken)
    {
        IEnumerable<SensorReading> readings = await sensorReadingQueryService.Handle(
            new GetSensorReadingsByDeviceIdQuery(iotDeviceId), cancellationToken);

        List<SensorReadingResource> resources = new List<SensorReadingResource>();
        foreach (SensorReading reading in readings)
            resources.Add(SensorReadingAssembler.ToResource(reading));

        return Ok(resources);
    }
}
