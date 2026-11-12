using System.ComponentModel.DataAnnotations;
using BottleTrack.API.Monitoring.Domain.Model.ValueObjects;

namespace BottleTrack.API.Monitoring.Interfaces.Rest.Resources;

public class RaiseAlertResource
{
    [Required]
    public Guid IotDeviceId { get; set; }

    [Required]
    public Guid TransportOperationId { get; set; }

    [Required]
    public AlertType Type { get; set; }
}
