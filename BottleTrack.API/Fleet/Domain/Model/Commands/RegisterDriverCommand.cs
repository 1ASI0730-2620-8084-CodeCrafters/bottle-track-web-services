namespace BottleTrack.API.Fleet.Domain.Model.Commands;

public class RegisterDriverCommand
{
    public Guid OrganizationId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string DocumentNumber { get; }

    public string LicenseNumber { get; }

    public string Phone { get; }

    public RegisterDriverCommand(Guid organizationId, string firstName, string lastName, string documentNumber,
        string licenseNumber, string phone)
    {
        OrganizationId = organizationId;
        FirstName = firstName;
        LastName = lastName;
        DocumentNumber = documentNumber;
        LicenseNumber = licenseNumber;
        Phone = phone;
    }
}
