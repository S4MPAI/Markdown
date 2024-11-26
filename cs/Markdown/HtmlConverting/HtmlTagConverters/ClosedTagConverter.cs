using System.Text;
using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class ClosedTagConverter(TagType tagType) : BaseHtmlTagConverter(tagType)
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
            var tagType = Token.GetTagType(token);

            if (tagType != null)
            {
                if (tagType.Value == HandledTag)
                {
                    stringBuilder.Append(HtmlTagsCreator.CreateCloseTag(HandledTag));
                    readTokens = i - start + 1;
                    return stringBuilder.ToString();
                }

                var convertedString = ConvertTokensToAnotherTag(converters, tokens, tagType.Value, ref i);
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