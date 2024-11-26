using System.Text;
using Markdown.HtmlConverting.HtmlTagConverters;
using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public class MarkdownToHtmlConverter : IMarkdownConverter
{
    private readonly Dictionary<TagType, IHtmlTagConverter> tagConverters = CreateTagConverters();

    private static Dictionary<TagType, IHtmlTagConverter> CreateTagConverters()
    {
        return new List<BaseHtmlTagConverter>
        {
            new HeaderTagConverter(),
            new ItalicTagConverter(),
            new StrongTagConverter()
        }.ToDictionary<BaseHtmlTagConverter, TagType, IHtmlTagConverter>(
            key => key.HandledTag, 
            value => value);
    }

    public string Convert(IEnumerable<Token> tokens)
    {
        IReadOnlyList<Token> convertedTokens = tokens.ToArray();
        var result = new StringBuilder();

        for (var i = 0; i < convertedTokens.Count; i++)
        {
            var token = convertedTokens[i];
            var tagType = Token.GetTagTypeByOpenTag(token);

            if (tagType == null) 
                continue;
            
            var converter = tagConverters[tagType.Value];
            var nextPosition = i + 1;
            var htmlText = converter.ConvertTokensToHtmlText(tagConverters,
                convertedTokens,
                nextPosition,
                out var readTokens);
            result.Append(htmlText);
            i += readTokens;
        }
        
        return result.ToString();
    }
}