namespace BottleTrack.API.Fleet.Domain.Model.Commands;

public class UpdateDriverCommand
{
    public Guid DriverId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Phone { get; }

    public UpdateDriverCommand(Guid driverId, string firstName, string lastName, string phone)
    {
        DriverId = driverId;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
    }
}
