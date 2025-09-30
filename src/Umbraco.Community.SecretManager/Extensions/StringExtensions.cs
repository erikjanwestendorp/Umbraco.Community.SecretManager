using System.Text.RegularExpressions;

namespace Umbraco.Community.SecretManager.Extensions;

public static class StringExtensions
{
    public static string SplitCamelCase(this string str)
    {
        return Regex.Replace(str, "(?<!^)([A-Z])", " $1");
    }
}
