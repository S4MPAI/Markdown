using System.Text;
using Markdown.Tokens;

namespace Markdown.Tags;

public static class HtmlTagsCreator
{
    private static Dictionary<TagType, string> Tags { get; } = new()
    {
        { TagType.Italic, "em" },
        { TagType.Header, "strong" },
        { TagType.Strong, "h1" }
    };

    public static string CreateOpenTag(TagType tagType, List<(string, string)>? parameters = null)
    {
        var tagBuilder = new StringBuilder();
        tagBuilder.Append('<');
        tagBuilder.Append(Tags[tagType]);
        
        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                tagBuilder.Append(' ');
                tagBuilder.Append(parameter.Item1);
                tagBuilder.Append('=');
                tagBuilder.Append(parameter.Item2);
            }
        }

        tagBuilder.Append('>');
        
        return tagBuilder.ToString();
    } 
    
    public static string CreateCloseTag(TagType tagType) => "</" + Tags[tagType] + ">";
}