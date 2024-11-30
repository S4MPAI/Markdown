using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class TagTokensIntersectsHandler(TagType firstTagType, TagType secondTagType) : ITokenHandler
{
    public int Priority => 6;
    
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = tokens.ToList();
        var tagTypesTokens = GetNeededTagTypesTokens(tokens);
        var firstTagTypePairs = GetTagsPairsOfNeededTag(tagTypesTokens, firstTagType);
        var secondTagTypePairs = GetTagsPairsOfNeededTag(tagTypesTokens, secondTagType);
        
        DisableIntersectsTags(handledTokens, firstTagTypePairs, secondTagTypePairs);
        DisableIntersectsTags(handledTokens, secondTagTypePairs, firstTagTypePairs);
        
        return handledTokens;
    }
    
    private List<(int position, Token token)> GetNeededTagTypesTokens(IReadOnlyList<Token> tokens) => 
        tokens.Select((t, i) => (position: i, token: t))
            .Where(tokenInfo => Token.IsTagToken(tokenInfo.token, firstTagType) || 
                                Token.IsTagToken(tokenInfo.token, secondTagType))
            .ToList();

    private static List<(int left, int right)> GetTagsPairsOfNeededTag(
        IReadOnlyList<(int position, Token token)> tagTokens,
        TagType tagType)
    {
        var openTags = new Queue<int>();
        var tagsPairs = new List<(int left, int right)>();

        foreach (var tokenInfo in tagTokens)
        {
            if (openTags.Count != 0 &&Token.TryGetTagTypeByCloseTag(tokenInfo.token, out var closeTagType) && 
                closeTagType == tagType)
            {
                tagsPairs.Add((openTags.Dequeue(), tokenInfo.position));
            }
            else if (Token.TryGetTagTypeByOpenTag(tokenInfo.token, out var openTagType) && openTagType == tagType)
            {
                openTags.Enqueue(tokenInfo.position);
            }
        }
        
        return tagsPairs;
    }
    
    private static void DisableIntersectsTags(
        List<Token> handledTokens,
        List<(int left, int right)> firstTagTypePairs, 
        List<(int left, int right)> secondTagTypePairs)
    {
        var pairIndex = 0;
        foreach (var firstTagTypePair in firstTagTypePairs)
        {
            while (pairIndex < secondTagTypePairs.Count && IsIntersects(firstTagTypePair, secondTagTypePairs[pairIndex]))
            {
                DisableTagTokenPair(handledTokens, secondTagTypePairs[pairIndex]);
                pairIndex++;
            }
            
            if (pairIndex >= secondTagTypePairs.Count)
                break;
        }
    }

    private static void DisableTagTokenPair(List<Token> handledTokens, (int left, int right) tagTokenPair)
    {
        var (left, right) = tagTokenPair;
        handledTokens[left] = Token.CreateTextToken(handledTokens[left].Content);
        handledTokens[right] = Token.CreateTextToken(handledTokens[right].Content);
    }

    private static bool IsIntersects((int left, int right) firstTagTypePairs, (int left, int right) secondTagTypePairs)
    {
        if (firstTagTypePairs.left > secondTagTypePairs.left)
            // ReSharper disable once TailRecursiveCall
            return IsIntersects(secondTagTypePairs, firstTagTypePairs);
        
        return firstTagTypePairs.right > secondTagTypePairs.left && 
               firstTagTypePairs.right < secondTagTypePairs.right &&
               secondTagTypePairs.left > firstTagTypePairs.left;
    }
}