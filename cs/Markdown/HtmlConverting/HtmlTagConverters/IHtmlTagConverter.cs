using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public interface IHtmlTagConverter
{
    public IList<Token> ConvertToHtml(IReadOnlyList<Token> tokens);
}