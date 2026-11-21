using System.Net.Mime;
using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Operations.Domain.Model.Queries;
using BottleTrack.API.Operations.Interfaces.Rest.Resources;
using BottleTrack.API.Operations.Interfaces.Rest.Transform;
using BottleTrack.API.Resources;
using BottleTrack.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BottleTrack.API.Operations.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("TransportOperations")]
public class TransportOperationsController : ControllerBase
{
    private readonly ITransportOperationCommandService transportOperationCommandService;
    private readonly ITransportOperationQueryService transportOperationQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;

    public TransportOperationsController(ITransportOperationCommandService transportOperationCommandService,
        ITransportOperationQueryService transportOperationQueryService, IStringLocalizer<SharedResource> localizer)
    {
        this.transportOperationCommandService = transportOperationCommandService;
        this.transportOperationQueryService = transportOperationQueryService;
        this.localizer = localizer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateTransportOperation([FromBody] CreateTransportOperationResource resource,
        CancellationToken cancellationToken)
    {
        var command = TransportOperationAssembler.ToCommand(resource);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            operation => CreatedAtAction(nameof(GetTransportOperationById), new { id = operation.Id },
                TransportOperationAssembler.ToResource(operation)),
            error => ErrorResult(error));
    }

    [HttpPost("{id:guid}/delivery-stops")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddDeliveryStop(Guid id, [FromBody] AddDeliveryStopResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AddDeliveryStopCommand(id, resource.DeliveryPointId, resource.SequenceNumber,
            resource.PlannedQuantity);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            operation => Ok(TransportOperationAssembler.ToResource(operation)),
            error => ErrorResult(error));
    }

    [HttpPost("{id:guid}/dispatches")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DispatchTransportOperation(Guid id,
        [FromBody] DispatchTransportOperationResource resource, CancellationToken cancellationToken)
    {
        var command = new DispatchTransportOperationCommand(id, resource.VehicleId, resource.DriverId);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);

        return result.Fold<ActionResult>(
            operation => Ok(TransportOperationAssembler.ToResource(operation)),
            error => ErrorResult(error));
    }

    [HttpPost("{id:guid}/delivery-stops/{deliveryStopId:guid}/moves")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> MoveDeliveryStop(Guid id, Guid deliveryStopId,
        [FromBody] MoveDeliveryStopResource resource, CancellationToken cancellationToken)
    {
        var command = new MoveDeliveryStopCommand(id, deliveryStopId, resource.Offset);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpPost("{id:guid}/vehicle-assignments")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AssignVehicle(Guid id, [FromBody] AssignVehicleResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AssignVehicleCommand(id, resource.VehicleId);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpPost("{id:guid}/driver-assignments")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AssignDriver(Guid id, [FromBody] AssignDriverResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AssignDriverCommand(id, resource.DriverId);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpPost("{id:guid}/starts")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> StartTransportOperation(Guid id, CancellationToken cancellationToken)
    {
        var command = new StartTransportOperationCommand(id);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpPost("{id:guid}/delivery-stops/{deliveryStopId:guid}/deliveries")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RecordDelivery(Guid id, Guid deliveryStopId,
        [FromBody] RecordDeliveryResource resource, CancellationToken cancellationToken)
    {
        var command = new RecordDeliveryCommand(id, deliveryStopId, resource.Status, resource.DeliveredQuantity,
            resource.FailureReason);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpPost("{id:guid}/completions")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CompleteTransportOperation(Guid id, CancellationToken cancellationToken)
    {
        var command = new CompleteTransportOperationCommand(id);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpPost("{id:guid}/cancellations")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CancelTransportOperation(Guid id,
        [FromBody] CancelTransportOperationResource resource, CancellationToken cancellationToken)
    {
        var command = new CancelTransportOperationCommand(id, resource.Reason);
        var result = await transportOperationCommandService.Handle(command, cancellationToken);
        return OperationResult(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TransportOperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTransportOperationById(Guid id, CancellationToken cancellationToken)
    {
        TransportOperation? operation = await transportOperationQueryService.Handle(
            new GetTransportOperationByIdQuery(id), cancellationToken);

        if (operation == null)
            return ErrorResult(TransportOperationError.TransportOperationNotFound);

        return Ok(TransportOperationAssembler.ToResource(operation));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransportOperationResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetTransportOperationsByOrganizationId([FromQuery] Guid organizationId,
        CancellationToken cancellationToken)
    {
        IEnumerable<TransportOperation> operations = await transportOperationQueryService.Handle(
            new GetTransportOperationsByOrganizationIdQuery(organizationId), cancellationToken);

        List<TransportOperationResource> resources = new List<TransportOperationResource>();
        foreach (TransportOperation operation in operations)
            resources.Add(TransportOperationAssembler.ToResource(operation));

        return Ok(resources);
    }

    private ActionResult OperationResult(Result<TransportOperation, TransportOperationError> result)
    {
        return result.Fold<ActionResult>(
            operation => Ok(TransportOperationAssembler.ToResource(operation)),
            error => ErrorResult(error));
    }

    private ActionResult ErrorResult(TransportOperationError error)
    {
        int statusCode = StatusCodes.Status400BadRequest;

        if (error == TransportOperationError.TransportOperationNotFound ||
            error == TransportOperationError.DeliveryStopNotFound)
            statusCode = StatusCodes.Status404NotFound;

        if (error == TransportOperationError.InvalidTransportOperationStatusChange ||
            error == TransportOperationError.VehicleNotAvailable ||
            error == TransportOperationError.DriverNotAvailable)
            statusCode = StatusCodes.Status409Conflict;

        return Problem(detail: localizer[error.ToString()].Value, statusCode: statusCode);
    }
}
