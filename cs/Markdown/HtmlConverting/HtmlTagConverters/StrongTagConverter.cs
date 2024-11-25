using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class StrongTagConverter : SymmetricTextTagConverter
{
    protected override TagType NeededTag => TagType.Strong;
}