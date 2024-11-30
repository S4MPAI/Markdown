using System.Text;
using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public static class HtmlTagsCreator
{
    private static Dictionary<TagType, string> Tags { get; } = new()
    {
        { TagType.Italic, "em" },
        { TagType.Header, "h1" },
        { TagType.Strong, "strong" }
    };

    public static string CreateOpenTag(TagType tagType, List<(string, string)>? parameters = null)
    {
        var tagBuilder = new StringBuilder();
        tagBuilder.Append('<');
        tagBuilder.Append(Tags[tagType]);
        
        foreach (var parameter in parameters ?? new())
        {
            tagBuilder.Append(' ');
            tagBuilder.Append(parameter.Item1);
            tagBuilder.Append('=');
            tagBuilder.Append(parameter.Item2);
        }

        tagBuilder.Append('>');
        
        return tagBuilder.ToString();
    } 
    
    public static string CreateCloseTag(TagType tagType) => "</" + Tags[tagType] + ">";
}