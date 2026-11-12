namespace BottleTrack.API.Fleet.Domain.Model.ValueObjects;

public sealed class PlateNumber
{
    public string Value { get; }

    public PlateNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The plate number is required.", nameof(value));

        string cleanedValue = value.Trim().ToUpperInvariant();

        if (cleanedValue.Length != 7 || cleanedValue[3] != '-')
            throw new ArgumentException("The plate number must follow the format AAA-111.", nameof(value));

        for (int index = 0; index < cleanedValue.Length; index++)
        {
            if (index == 3)
                continue;

            if (!char.IsLetterOrDigit(cleanedValue[index]))
                throw new ArgumentException("The plate number must contain only letters and digits.", nameof(value));
        }

        Value = cleanedValue;
    }

    public override bool Equals(object? obj)
    {
        PlateNumber? other = obj as PlateNumber;
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
