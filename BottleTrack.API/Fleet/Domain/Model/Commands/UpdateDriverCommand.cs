using BottleTrack.API.Fleet.Domain.Model.ValueObjects;

namespace BottleTrack.API.Fleet.Domain.Model.Commands;

public class UpdateDriverCommand
{
    public Guid DriverId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Phone { get; }

    public DriverStatus? Status { get; }

    public UpdateDriverCommand(Guid driverId, string firstName, string lastName, string phone, DriverStatus? status)
    {
        DriverId = driverId;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Status = status;
    }
}
