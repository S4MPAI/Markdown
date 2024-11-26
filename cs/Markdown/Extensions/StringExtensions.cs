namespace Markdown.Extensions;

public static class StringExtensions
{
    public static string GetChar(this string str, int index) => 
        str.Substring(index, 1);
}