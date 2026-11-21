using System.ComponentModel.DataAnnotations;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class MoveDeliveryStopResource
{
    [Range(-1, 1)]
    public int Offset { get; set; }
}
