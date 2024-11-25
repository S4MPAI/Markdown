using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class ItalicTagConverter : SymmetricTextTagConverter
{
    protected override TagType NeededTag => TagType.Italic;
}