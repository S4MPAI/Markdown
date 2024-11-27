using System.Text;
using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class OpenedTagConverter(TagType tagType, TokenType endTokenType) : BaseHtmlTagConverter(tagType)
{
    public override string ConvertTokensToHtmlText(
        Dictionary<TagType, IHtmlTagConverter> converters,
        IReadOnlyList<Token> tokens,
        int start,
        out int readTokens)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(HtmlTagsCreator.CreateOpenTag(HandledTag));
        readTokens = 0;
        
        for (var i = start; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.Type == endTokenType)
            {
                stringBuilder.Append(HtmlTagsCreator.CreateCloseTag(HandledTag));
                readTokens = i - start + 1;
                return stringBuilder.ToString();
            }
            
            if (Token.TryGetTagTypeByOpenTag(token, out var tagType))
            {
                var convertedString = ConvertTokensToHtmlTextInTag(converters, tokens, tagType, ref i);
                stringBuilder.Append(convertedString);
            }
            else 
            {
                stringBuilder.Append(token.Content);
            }
        }
        
        readTokens = tokens.Count - start;
        return stringBuilder.ToString();
    }
}