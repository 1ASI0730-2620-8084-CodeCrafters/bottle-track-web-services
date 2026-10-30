using System.Text.RegularExpressions;

namespace BottleTrack.API.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;

public static class StringExtensions
{
    private static readonly Regex KebabCaseRegex = new Regex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled);

    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return KebabCaseRegex.Replace(text, "-$1").Trim().ToLower();
    }
}
