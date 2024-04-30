namespace System;

internal static class ParseExtensions
{
    public static IEnumerable<long> ParseLongsSafely(this string[] strings)
    {
        if (strings.Length == 1)
            strings = strings[0].Split(',');
        
        foreach (var s in strings)
            if (long.TryParse(s, out var value))
                yield return value;
    }
}
