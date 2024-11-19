using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public interface IMarkdownConverter
{
    public string Convert(IEnumerable<Token> tokens);
}