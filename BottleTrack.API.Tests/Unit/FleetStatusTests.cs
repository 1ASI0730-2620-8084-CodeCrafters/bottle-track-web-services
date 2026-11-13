using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class FleetStatusTests
{
    private static Vehicle NewVehicle()
    {
        return Vehicle.Register(Guid.NewGuid(), new PlateNumber("ABC-123"), "Volvo", "FH 460", 400);
    }

    [Fact]
    public void SendToMaintenance_ThenReturnToService_RestoresAvailability()
    {
        Vehicle vehicle = NewVehicle();

        vehicle.SendToMaintenance();
        Assert.Equal(VehicleStatus.InMaintenance, vehicle.Status);

        vehicle.ReturnToService();
        Assert.Equal(VehicleStatus.Available, vehicle.Status);
    }

    [Fact]
    public void ReturnToService_FailsWhenVehicleIsNotInMaintenance()
    {
        Vehicle vehicle = NewVehicle();

        Assert.Throws<InvalidOperationException>(() => vehicle.ReturnToService());
    }

    [Fact]
    public void SendToMaintenance_FailsWhenVehicleIsAssigned()
    {
        Vehicle vehicle = NewVehicle();
        vehicle.Reserve();

        Assert.Throws<InvalidOperationException>(() => vehicle.SendToMaintenance());
    }
}
