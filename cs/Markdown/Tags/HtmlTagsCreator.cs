using System.Text;

namespace Markdown.Tags;

public static class HtmlTagsCreator
{
    private static Dictionary<string, string> Tags { get; } = new()
    {
        { "_", "em" },
        { "__", "strong" },
        { "#", "h1" }
    };

    private static readonly List<ITagFactory> SpecificTagsFactories = GetTagsFactories();

    private static List<ITagFactory> GetTagsFactories()
    {
        return [];
    }

    public static string TryConvertMdTag(string tagValue)
    {
        Tags.TryGetValue(tagValue, out var tag);
        
        if (tag != null)
            return tag;

        foreach (var tagFactory in SpecificTagsFactories)
        {
            tag = tagFactory.TryConvertTag(tagValue);
            
            if (tag != null)
                return tag;
        }
        
        return tag;
    }

    public static string CreateOpenTag(string tagName, List<(string, string)>? parameters = null)
    {
        var tagBuilder = new StringBuilder();
        tagBuilder.Append('<');
        tagBuilder.Append(tagName);
        
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
    
    public static string CreateCloseTag(string tagName) => "</" + tagName + ">";
}