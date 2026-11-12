using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Domain.Model.Aggregates;

public class Organization
{
    public Guid Id { get; private set; }

    public string BusinessName { get; private set; }

    public TaxId TaxId { get; private set; }

    public EmailAddress Email { get; private set; }

    public string Phone { get; private set; }

    public string Address { get; private set; }

    private Organization()
    {
        BusinessName = string.Empty;
        TaxId = null!;
        Email = null!;
        Phone = string.Empty;
        Address = string.Empty;
    }

    public static Organization Register(string businessName, TaxId taxId, EmailAddress email)
    {
        Organization organization = new Organization();
        organization.Id = Guid.NewGuid();
        organization.BusinessName = businessName.Trim();
        organization.TaxId = taxId;
        organization.Email = email;
        return organization;
    }

    public void UpdateProfile(string businessName, string phone, string address)
    {
        BusinessName = businessName.Trim();
        Phone = phone.Trim();
        Address = address.Trim();
    }
}
