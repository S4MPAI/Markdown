using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class ItalicTagConverter : BaseTextTagConverter
{
    protected override TagType NeededTag => TagType.Italic;
}