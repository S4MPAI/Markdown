namespace Markdown.Tags;

public interface ITagFactory
{
    public string? TryConvertTag(string tagValue);
}