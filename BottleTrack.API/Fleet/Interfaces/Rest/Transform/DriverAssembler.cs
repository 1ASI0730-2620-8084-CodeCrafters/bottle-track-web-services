using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Commands;
using BottleTrack.API.Fleet.Interfaces.Rest.Resources;

namespace BottleTrack.API.Fleet.Interfaces.Rest.Transform;

public static class DriverAssembler
{
    public static RegisterDriverCommand ToCommand(RegisterDriverResource resource)
    {
        return new RegisterDriverCommand(resource.OrganizationId, resource.FirstName, resource.LastName,
            resource.DocumentNumber, resource.LicenseNumber, resource.Phone);
    }

    public static DriverResource ToResource(Driver driver)
    {
        DriverResource resource = new DriverResource();
        resource.Id = driver.Id;
        resource.OrganizationId = driver.OrganizationId;
        resource.FirstName = driver.FirstName;
        resource.LastName = driver.LastName;
        resource.DocumentNumber = driver.DocumentNumber;
        resource.LicenseNumber = driver.LicenseNumber.Value;
        resource.Phone = driver.Phone;
        resource.Status = driver.Status;
        return resource;
    }
}
