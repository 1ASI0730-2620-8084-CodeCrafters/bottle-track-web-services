using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Operations.Application.Services;

public interface IDeliveryPointCommandService
{
    Task<Result<DeliveryPoint, DeliveryPointError>> Handle(RegisterDeliveryPointCommand command,
        CancellationToken cancellationToken = default);
}
