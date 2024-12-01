using System.Text;
using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class LinkTagConverter() : BaseHtmlTagConverter(TagType.LinkText)
{
    public override string ConvertTokensToHtmlText(
        Dictionary<TagType, IHtmlTagConverter> converters,
        IReadOnlyList<Token> tokens,
        int start,
        out int readTokens)
    {
        var linkTextBuilder = new StringBuilder();
        var currentPosition = start;

        while (!IsCloseTagOfNeededTagType(tokens, currentPosition, TagType.LinkText))
            linkTextBuilder.Append(tokens[currentPosition++].Content);

        readTokens = currentPosition - start;
        currentPosition++;
        if (currentPosition >= tokens.Count ||
            tokens[currentPosition].Type != TokenType.Tag ||
            Token.TryGetTagTypeByOpenTag(tokens[currentPosition], out var nextTagType) ||
            nextTagType != TagType.LinkValue)
        {
            return linkTextBuilder.ToString();
        }

        var linkValueBuilder = new StringBuilder();
        currentPosition++;
        while (!IsCloseTagOfNeededTagType(tokens, currentPosition, TagType.LinkValue))
            linkValueBuilder.Append(tokens[currentPosition++].Content);

        return HtmlTagsCreator.CreateOpenTag(TagType.LinkValue, ("href", linkValueBuilder.ToString())) +
               linkTextBuilder +
               HtmlTagsCreator.CreateCloseTag(TagType.LinkValue);
}

    private static bool IsCloseTagOfNeededTagType(IReadOnlyList<Token> tokens, int position, TagType neededTagType)
    {
        var token = tokens[position];
        return token.Type == TokenType.Tag &&
               Token.TryGetTagTypeByCloseTag(token, out var tagType) &&
               tagType == neededTagType;
    }
}