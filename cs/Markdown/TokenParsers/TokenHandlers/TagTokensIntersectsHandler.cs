using Markdown.Extensions;
using Markdown.Helpers;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class TagTokensIntersectsHandler(TagType firstTagType, TagType secondTagType) : ITokenHandler
{
    public int Priority => 6;
    
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = tokens.ToList();
        var tagTypesTokens = tokens.GetNeededTagTypesTokens(firstTagType, secondTagType).ToList();
        var firstTagTypePairs = tagTypesTokens.GetTagsPairsOfNeededTag(firstTagType).ToList();
        var secondTagTypePairs = tagTypesTokens.GetTagsPairsOfNeededTag(secondTagType).ToList();
        
        DisableIntersectsTags(handledTokens, firstTagTypePairs, secondTagTypePairs);
        DisableIntersectsTags(handledTokens, secondTagTypePairs, firstTagTypePairs);
        
        return handledTokens;
    }
    
    private static void DisableIntersectsTags(
        List<Token> handledTokens,
        IReadOnlyList<(int left, int right)> firstTagTypePairs, 
        IReadOnlyList<(int left, int right)> secondTagTypePairs)
    {
        var pairIndex = 0;
        foreach (var firstTagTypePair in firstTagTypePairs)
        {
            while (pairIndex < secondTagTypePairs.Count && 
                   IntervalHelper.IsIntersects(firstTagTypePair, secondTagTypePairs[pairIndex]))
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
}