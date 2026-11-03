namespace BottleTrack.API.Iam.Domain.Model.ValueObjects;

public sealed class TaxId
{
    private const int RequiredLength = 11;

    public string Value { get; }

    public TaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The tax id is required.", nameof(value));

        string cleanedValue = value.Trim();

        if (cleanedValue.Length != RequiredLength)
            throw new ArgumentException("The tax id must have 11 digits.", nameof(value));

        foreach (char character in cleanedValue)
        {
            if (!char.IsDigit(character))
                throw new ArgumentException("The tax id must contain only digits.", nameof(value));
        }

        Value = cleanedValue;
    }

    public override bool Equals(object? obj)
    {
        TaxId? other = obj as TaxId;
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
