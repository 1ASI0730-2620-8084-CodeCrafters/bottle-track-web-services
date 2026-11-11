using BottleTrack.API.Operations.Application.Errors;
using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Operations.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Operations.Application.Internal.CommandServices;

public class DeliveryPointCommandService : IDeliveryPointCommandService
{
    private readonly IDeliveryPointRepository deliveryPointRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeliveryPointCommandService(IDeliveryPointRepository deliveryPointRepository, IUnitOfWork unitOfWork)
    {
        this.deliveryPointRepository = deliveryPointRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<DeliveryPoint, DeliveryPointError>> Handle(RegisterDeliveryPointCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.BusinessName) || string.IsNullOrWhiteSpace(command.Address) ||
            string.IsNullOrWhiteSpace(command.District))
            return Result<DeliveryPoint, DeliveryPointError>.Failure(DeliveryPointError.InvalidDeliveryPointData);

        DeliveryPoint deliveryPoint = DeliveryPoint.Register(command.OrganizationId, command.OwnerUserId,
            command.BusinessName, command.Address, command.District, command.Latitude, command.Longitude);

        await deliveryPointRepository.AddAsync(deliveryPoint, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<DeliveryPoint, DeliveryPointError>.Success(deliveryPoint);
    }
}
