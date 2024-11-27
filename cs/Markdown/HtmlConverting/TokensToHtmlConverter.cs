using System.Text;
using Markdown.HtmlConverting.HtmlTagConverters;
using Markdown.MdParsing.Interfaces;
using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public class TokensToHtmlConverter : ITokensConverter
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

            if (!Token.TryGetTagTypeByOpenTag(token, out var tagType)) 
                continue;
            
            var converter = tagConverters[tagType];
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