using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class StrongTagConverter : BaseTextTagConverter
{
    protected override TagType NeededTag => TagType.Strong;
}