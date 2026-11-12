using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Application.Internal.CommandServices;

public class IotDeviceCommandService : IIotDeviceCommandService
{
    private readonly IIotDeviceRepository iotDeviceRepository;
    private readonly IUnitOfWork unitOfWork;

    public IotDeviceCommandService(IIotDeviceRepository iotDeviceRepository, IUnitOfWork unitOfWork)
    {
        this.iotDeviceRepository = iotDeviceRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<IotDevice, IotDeviceError>> Handle(RegisterIotDeviceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.SerialNumber) ||
            command.SafeTemperatureMin > command.SafeTemperatureMax)
            return Result<IotDevice, IotDeviceError>.Failure(IotDeviceError.InvalidIotDeviceData);

        string serialNumber = command.SerialNumber.Trim().ToUpperInvariant();
        IotDevice? existingDevice = await iotDeviceRepository.FindBySerialNumberAsync(serialNumber, cancellationToken);
        if (existingDevice != null)
            return Result<IotDevice, IotDeviceError>.Failure(IotDeviceError.SerialNumberAlreadyRegistered);

        IotDevice device = IotDevice.Register(command.SerialNumber, command.VehicleId, command.SafeTemperatureMin,
            command.SafeTemperatureMax, command.ImpactThreshold);

        await iotDeviceRepository.AddAsync(device, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<IotDevice, IotDeviceError>.Success(device);
    }
}
