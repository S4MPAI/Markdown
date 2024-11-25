using System.Text;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class OpenedTagConverter(TagType tagType, TokenType endTokenType) : BaseHtmlTagConverter(tagType)
{
    public override string ConvertTokensToHtmlText(
        Dictionary<TagType, IHtmlTagConverter> converters,
        IReadOnlyList<Token> tokens,
        int start,
        out int readedTokens)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(HtmlTagsCreator.CreateOpenTag(HandledTag));
        readedTokens = 0;
        
        for (var i = start; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.Type == endTokenType)
            {
                stringBuilder.Append(HtmlTagsCreator.CreateCloseTag(HandledTag));
                readedTokens = i - start + 1;
                return stringBuilder.ToString();
            }
            
            var tagType = Token.TryGetTagType(token);
            if (tagType != null)
            {
                var convertedString = ConvertTokensToAnotherTag(converters, tokens, tagType.Value, ref i);
                stringBuilder.Append(convertedString);
            }
            else 
            {
                stringBuilder.Append(token.Content);
            }
        }
        
        readedTokens = tokens.Count - start;
        return stringBuilder.ToString();
    }
}