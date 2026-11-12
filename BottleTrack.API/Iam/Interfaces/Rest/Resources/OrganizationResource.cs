namespace BottleTrack.API.Iam.Interfaces.Rest.Resources;

public class OrganizationResource
{
    public Guid Id { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string TaxId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}
