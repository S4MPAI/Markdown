using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public interface IHtmlTagConverter
{
    public IList<Token> ConvertToHtml(IList<Token> tokens);
}