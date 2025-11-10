using System.Text.RegularExpressions;

namespace Examples.Extensions;

public static partial class LetterCaseStringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        return camelCaseExpression().Replace(input, "$1_$2").ToLower();
    }

    [GeneratedRegex(@"([a-z0-9])([A-Z])", RegexOptions.Compiled)]
    private static partial Regex camelCaseExpression();
}
