using BottleTrack.API.Monitoring.Application.Errors;
using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Model.Aggregates;
using BottleTrack.API.Monitoring.Domain.Model.Commands;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Monitoring.Application.Internal.CommandServices;

public class SensorReadingCommandService : ISensorReadingCommandService
{
    private readonly ISensorReadingRepository sensorReadingRepository;
    private readonly IIotDeviceRepository iotDeviceRepository;
    private readonly IUnitOfWork unitOfWork;

    public SensorReadingCommandService(ISensorReadingRepository sensorReadingRepository,
        IIotDeviceRepository iotDeviceRepository, IUnitOfWork unitOfWork)
    {
        this.sensorReadingRepository = sensorReadingRepository;
        this.iotDeviceRepository = iotDeviceRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<SensorReading, SensorReadingError>> Handle(RecordSensorReadingCommand command,
        CancellationToken cancellationToken = default)
    {
        IotDevice? device = await iotDeviceRepository.FindByIdAsync(command.IotDeviceId, cancellationToken);
        if (device == null)
            return Result<SensorReading, SensorReadingError>.Failure(SensorReadingError.InvalidSensorReadingData);

        SensorReading reading = SensorReading.Record(command.IotDeviceId, command.TransportOperationId,
            command.RecordedAt, command.Latitude, command.Longitude, command.TemperatureCelsius, command.ImpactForce);

        await sensorReadingRepository.AddAsync(reading, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<SensorReading, SensorReadingError>.Success(reading);
    }
}
