namespace BottleTrack.API.Fleet.Domain.Model.ValueObjects;

public sealed class LicenseNumber
{
    public string Value { get; }

    public LicenseNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The license number is required.", nameof(value));

        string cleanedValue = value.Trim().ToUpperInvariant();

        if (cleanedValue.Length != 9 || !char.IsLetter(cleanedValue[0]))
            throw new ArgumentException("The license number must be one letter followed by 8 digits.", nameof(value));

        for (int index = 1; index < cleanedValue.Length; index++)
        {
            if (!char.IsDigit(cleanedValue[index]))
                throw new ArgumentException("The license number must be one letter followed by 8 digits.", nameof(value));
        }

        Value = cleanedValue;
    }

    public override bool Equals(object? obj)
    {
        LicenseNumber? other = obj as LicenseNumber;
        if (other == null)
            return false;

        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}
