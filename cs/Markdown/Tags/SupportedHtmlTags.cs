namespace Markdown.Tags;

public static class SupportedHtmlTags
{
    private static Dictionary<string, Tag> Tags { get; } = new()
    {
        { "_", new Tag("_", "em", true) },
        { "__", new Tag("__", "strong", true) },
        { "#", new Tag("#", "h1", true) }
    };

    private static readonly List<ITagFactory> SpecificTagsFactories = GetTagsFactories();

    private static List<ITagFactory> GetTagsFactories()
    {
        return [];
    }

    public static Tag? TryGetTag(string tagValue)
    {
        Tags.TryGetValue(tagValue, out var tag);
        
        if (tag != null)
            return tag;

        foreach (var tagFactory in SpecificTagsFactories)
        {
            tag = tagFactory.TryCreateTag(tagValue);
            
            if (tag != null)
                break;
        }
        
        return tag;
    }
}