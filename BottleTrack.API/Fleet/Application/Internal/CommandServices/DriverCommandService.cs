using BottleTrack.API.Fleet.Application.Errors;
using BottleTrack.API.Fleet.Application.Services;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Commands;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Fleet.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Fleet.Application.Internal.CommandServices;

public class DriverCommandService : IDriverCommandService
{
    private const int DocumentNumberLength = 8;

    private readonly IDriverRepository driverRepository;
    private readonly IUnitOfWork unitOfWork;

    public DriverCommandService(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
    {
        this.driverRepository = driverRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Driver, DriverError>> Handle(RegisterDriverCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
            return Result<Driver, DriverError>.Failure(DriverError.InvalidDriverData);

        if (!IsValidDocumentNumber(command.DocumentNumber))
            return Result<Driver, DriverError>.Failure(DriverError.InvalidDocumentNumber);

        LicenseNumber licenseNumber;
        try
        {
            licenseNumber = new LicenseNumber(command.LicenseNumber);
        }
        catch (ArgumentException)
        {
            return Result<Driver, DriverError>.Failure(DriverError.InvalidLicenseNumber);
        }

        string documentNumber = command.DocumentNumber.Trim();

        Driver? driverWithDocument = await driverRepository.FindByDocumentNumberAsync(documentNumber, cancellationToken);
        if (driverWithDocument != null)
            return Result<Driver, DriverError>.Failure(DriverError.DocumentNumberAlreadyRegistered);

        Driver? driverWithLicense = await driverRepository.FindByLicenseNumberAsync(licenseNumber, cancellationToken);
        if (driverWithLicense != null)
            return Result<Driver, DriverError>.Failure(DriverError.LicenseNumberAlreadyRegistered);

        Driver driver = Driver.Register(command.OrganizationId, command.FirstName, command.LastName, documentNumber,
            licenseNumber, command.Phone);

        await driverRepository.AddAsync(driver, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Driver, DriverError>.Success(driver);
    }

    public async Task<Result<Driver, DriverError>> Handle(UpdateDriverCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
            return Result<Driver, DriverError>.Failure(DriverError.InvalidDriverData);

        Driver? driver = await driverRepository.FindByIdAsync(command.DriverId, cancellationToken);
        if (driver == null)
            return Result<Driver, DriverError>.Failure(DriverError.DriverNotFound);

        driver.UpdateDetails(command.FirstName, command.LastName, command.Phone);

        driverRepository.Update(driver);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Driver, DriverError>.Success(driver);
    }

    private bool IsValidDocumentNumber(string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return false;

        string cleanedValue = documentNumber.Trim();
        if (cleanedValue.Length != DocumentNumberLength)
            return false;

        foreach (char character in cleanedValue)
        {
            if (!char.IsDigit(character))
                return false;
        }

        return true;
    }
}
