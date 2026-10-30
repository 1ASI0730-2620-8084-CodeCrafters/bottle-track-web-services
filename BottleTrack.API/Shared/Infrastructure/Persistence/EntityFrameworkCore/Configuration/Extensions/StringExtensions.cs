using System.Text;
using Humanizer;

namespace BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string text)
    {
        StringBuilder builder = new StringBuilder();

        for (int index = 0; index < text.Length; index++)
        {
            char character = text[index];

            if (char.IsUpper(character) && index > 0)
                builder.Append('_');

            builder.Append(char.ToLower(character));
        }

        return builder.ToString();
    }

    public static string ToPlural(this string text)
    {
        return text.Pluralize(false);
    }
}
