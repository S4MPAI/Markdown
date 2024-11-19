namespace Markdown.Tags;

public interface ITagFactory
{
    public Tag? TryCreateTag(string tagValue);
}