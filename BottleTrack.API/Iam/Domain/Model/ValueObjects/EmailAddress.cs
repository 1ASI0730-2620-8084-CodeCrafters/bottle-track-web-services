namespace BottleTrack.API.Iam.Domain.Model.ValueObjects;

public sealed class EmailAddress
{
    private const int MaxLength = 255;

    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The email address is required.", nameof(value));

        string cleanedValue = value.Trim().ToLowerInvariant();

        if (cleanedValue.Length > MaxLength)
            throw new ArgumentException("The email address is too long.", nameof(value));

        int atIndex = cleanedValue.IndexOf('@');
        if (atIndex <= 0 || atIndex == cleanedValue.Length - 1)
            throw new ArgumentException("The email address is not valid.", nameof(value));

        Value = cleanedValue;
    }

    public override bool Equals(object? obj)
    {
        EmailAddress? other = obj as EmailAddress;
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
