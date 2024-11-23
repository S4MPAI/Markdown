using System.Text;
using Markdown.HtmlConverting.HtmlTagConverters;
using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public class MarkdownToHtmlConverter : IMarkdownConverter
{
    private readonly List<IHtmlTagConverter> tagConverters = CreateTagConverters();

    private static List<IHtmlTagConverter> CreateTagConverters()
    {
        return new List<IHtmlTagConverter>
        {
            new HeaderTagConverter(),
            new ItalicTagConverter(),
            new StrongTagConverter()
        };
    }

    public string Convert(IEnumerable<Token> tokens)
    {
        IList<Token> convertedTokens = tokens.ToArray();
        var result = new StringBuilder();
        
        foreach (var tagConverter in tagConverters)
            convertedTokens = tagConverter.ConvertToHtml(convertedTokens);

        foreach (var text in convertedTokens.Select(token => token.Content))
            result.Append(text);
        
        return result.ToString();
    }
}