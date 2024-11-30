using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokensIEnumerableExtensions
{
    public static IEnumerable<(int position, Token token)> GetNeededTagTypesTokens(
        this IEnumerable<Token> tokens,
        params TagType[] neededTagTypes) => 
        tokens
            .Select((t, i) => (position: i, token: t))
            .Where(tokenInfo => neededTagTypes.Any(type => Token.IsTagToken(tokenInfo.token, type)));

    public static IEnumerable<(int left, int right)> GetTagsPairsOfNeededTag(
        this IEnumerable<(int position, Token token)> tagTokens,
        TagType tagType)
    {
        var openTags = new Queue<int>();

        foreach (var tokenInfo in tagTokens)
        {
            if (openTags.Count != 0 &&Token.TryGetTagTypeByCloseTag(tokenInfo.token, out var closeTagType) && 
                closeTagType == tagType)
            {
                yield return (openTags.Dequeue(), tokenInfo.position);
            }
            else if (Token.TryGetTagTypeByOpenTag(tokenInfo.token, out var openTagType) && openTagType == tagType)
            {
                openTags.Enqueue(tokenInfo.position);
            }
        }
    }
}