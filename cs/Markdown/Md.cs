using Markdown.HtmlConverting;
using Markdown.TokenParsers;

namespace Markdown;

public class Md
{
    private readonly MarkdownTokenParser markdownTokenParser = new();
    private readonly IMarkdownConverter converter = new MarkdownToHtmlConverter();

    public string Render(string text)
    {
        var tokens = markdownTokenParser.Parse(text);
        
        return converter.Convert(tokens);
    }
}