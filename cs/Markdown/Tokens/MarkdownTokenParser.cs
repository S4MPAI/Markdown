using Markdown.Tags;
using Markdown.TokenHandlers;

namespace Markdown.Tokens;

public class MarkdownTokenParser
{
    private readonly List<ITokenHandler> tokenHandlers = CreateHandlers();

    private static List<ITokenHandler> CreateHandlers() => [];

    public IEnumerable<Token> Parse(string text)
    {
        var tokens = ParseStringOnCommonTokens(text).ToList();
        
        foreach (var tokenHandler in tokenHandlers)
            tokens = tokenHandler.Handle(tokens).ToList();
        
        return tokens;
    }

    private static IEnumerable<Token> ParseStringOnCommonTokens(string text)
    {
        var left = 0;
        
        for (var i = 0; i <= text.Length; i++)
        {
            var nextToken = TryGetNextToken(text[i]);

            if (nextToken == null) 
                continue;
            
            var wordLength = i - left;
            if (wordLength > 0)
                yield return new Token(text.Substring(left, wordLength), TokenType.Word);

            left = i + 1;
                
            yield return nextToken.Value;
        }

        if (left < text.Length)
            yield return new Token(text.Substring(left, text.Length - left), TokenType.Word);
    }

    private static Token? TryGetNextToken(char currentChar)
    {
        if (char.IsLetter(currentChar))
            return null;
        
        if (char.IsDigit(currentChar))
            return new Token(currentChar, TokenType.Digit);
        
        var isTag = SupportedHtmlTags.TryGetTag(currentChar.ToString()) != null;
        
        if (isTag)
            return new Token(currentChar, TokenType.Tag);

        return currentChar switch
        {
            ' ' => new Token(currentChar, TokenType.Space),
            '\n' => new Token(currentChar, TokenType.NewLine),
            '\r' => new Token(currentChar, TokenType.NewLine),
            '\\' => new Token(currentChar, TokenType.Escape),
            _ => throw new ArgumentOutOfRangeException(
                nameof(currentChar), 
                currentChar, 
                $"{nameof(currentChar)} not supported for parsing")
        };
    }
}