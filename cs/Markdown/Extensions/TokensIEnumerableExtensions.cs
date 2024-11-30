using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokensIEnumerableExtensions
{
    public static IEnumerable<(int position, Token token)> GetTagTypesTokens(
        this IEnumerable<Token> tokens,
        params TagType[] tagTypes) => 
        tokens
            .Select((t, i) => (position: i, token: t))
            .Where(tokenInfo => tagTypes.Any(type => Token.IsTagToken(tokenInfo.token, type)));

    public static IEnumerable<(int left, int right)> GetTagsPairsOfTag(
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

    public static void DisableTagTokensPairsBy(
        this IList<Token> tokens,
        IReadOnlyList<(int left, int right)> checkPairs,
        IReadOnlyList<(int left, int right)> disablePairs,
        Func<(int left, int right), (int left, int right), bool> predicate)
    {
        var pairIndex = 0;
        foreach (var firstPair in checkPairs)
        {
            while (pairIndex < disablePairs.Count && predicate(firstPair, disablePairs[pairIndex]))
            {
                DisableTagTokenPair(tokens, disablePairs[pairIndex]);
                pairIndex++;
            }
        }
    }
    
    private static void DisableTagTokenPair(IList<Token> handledTokens, (int left, int right) tagTokenPair)
    {
        var (left, right) = tagTokenPair;
        handledTokens[left] = Token.CreateTextToken(handledTokens[left].Content);
        handledTokens[right] = Token.CreateTextToken(handledTokens[right].Content);
    }
}