using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Resources;

public class DriverResource
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string DocumentNumber { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DriverStatus Status { get; set; }
}
