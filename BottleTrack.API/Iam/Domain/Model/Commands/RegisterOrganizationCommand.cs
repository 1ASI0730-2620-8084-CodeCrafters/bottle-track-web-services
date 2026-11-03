namespace BottleTrack.API.Iam.Domain.Model.Commands;

public class RegisterOrganizationCommand
{
    public string BusinessName { get; }

    public string TaxId { get; }

    public string Email { get; }

    public string Phone { get; }

    public string Address { get; }

    public RegisterOrganizationCommand(string businessName, string taxId, string email, string phone, string address)
    {
        BusinessName = businessName;
        TaxId = taxId;
        Email = email;
        Phone = phone;
        Address = address;
    }
}
