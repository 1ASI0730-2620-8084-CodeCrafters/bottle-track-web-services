using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Domain.Model.Aggregates;

public class Driver
{
    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string DocumentNumber { get; private set; }

    public LicenseNumber LicenseNumber { get; private set; }

    public string Phone { get; private set; }

    public DriverStatus Status { get; private set; }

    private Driver()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        DocumentNumber = string.Empty;
        LicenseNumber = null!;
        Phone = string.Empty;
    }

    public static Driver Register(Guid organizationId, string firstName, string lastName, string documentNumber,
        LicenseNumber licenseNumber, string phone)
    {
        Driver driver = new Driver();
        driver.Id = Guid.NewGuid();
        driver.OrganizationId = organizationId;
        driver.FirstName = firstName.Trim();
        driver.LastName = lastName.Trim();
        driver.DocumentNumber = documentNumber.Trim();
        driver.LicenseNumber = licenseNumber;
        driver.Phone = phone.Trim();
        driver.Status = DriverStatus.Available;
        return driver;
    }

    public void UpdateDetails(string firstName, string lastName, string phone)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone.Trim();
    }

    public void Reserve()
    {
        if (Status != DriverStatus.Available)
            throw new InvalidOperationException("Only an available driver can be reserved.");

        Status = DriverStatus.Assigned;
    }

    public void Release()
    {
        if (Status != DriverStatus.Assigned)
            throw new InvalidOperationException("Only an assigned driver can be released.");

        Status = DriverStatus.Available;
    }

    public void Deactivate()
    {
        if (Status != DriverStatus.Available)
            throw new InvalidOperationException("Only an available driver can be deactivated.");

        Status = DriverStatus.Inactive;
    }

    public void Activate()
    {
        if (Status != DriverStatus.Inactive)
            throw new InvalidOperationException("Only an inactive driver can be activated.");

        Status = DriverStatus.Available;
    }
}
