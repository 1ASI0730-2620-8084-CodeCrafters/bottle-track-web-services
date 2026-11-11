using BottleTrack.API.Fleet.Application.Errors;
using BottleTrack.API.Fleet.Application.Services;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Commands;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Fleet.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Fleet.Application.Internal.CommandServices;

public class VehicleCommandService : IVehicleCommandService
{
    private readonly IVehicleRepository vehicleRepository;
    private readonly IUnitOfWork unitOfWork;

    public VehicleCommandService(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        this.vehicleRepository = vehicleRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Vehicle, VehicleError>> Handle(RegisterVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Brand) || string.IsNullOrWhiteSpace(command.Model) ||
            command.CapacityInBoxes <= 0)
            return Result<Vehicle, VehicleError>.Failure(VehicleError.InvalidVehicleData);

        PlateNumber plateNumber;
        try
        {
            plateNumber = new PlateNumber(command.PlateNumber);
        }
        catch (ArgumentException)
        {
            return Result<Vehicle, VehicleError>.Failure(VehicleError.InvalidPlateNumber);
        }

        Vehicle? existingVehicle = await vehicleRepository.FindByPlateNumberAsync(plateNumber, cancellationToken);
        if (existingVehicle != null)
            return Result<Vehicle, VehicleError>.Failure(VehicleError.PlateNumberAlreadyRegistered);

        Vehicle vehicle = Vehicle.Register(command.OrganizationId, plateNumber, command.Brand, command.Model,
            command.CapacityInBoxes);

        await vehicleRepository.AddAsync(vehicle, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Vehicle, VehicleError>.Success(vehicle);
    }

    public async Task<Result<Vehicle, VehicleError>> Handle(UpdateVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Brand) || string.IsNullOrWhiteSpace(command.Model) ||
            command.CapacityInBoxes <= 0)
            return Result<Vehicle, VehicleError>.Failure(VehicleError.InvalidVehicleData);

        Vehicle? vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle == null)
            return Result<Vehicle, VehicleError>.Failure(VehicleError.VehicleNotFound);

        vehicle.UpdateDetails(command.Brand, command.Model, command.CapacityInBoxes);

        vehicleRepository.Update(vehicle);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Vehicle, VehicleError>.Success(vehicle);
    }
}
