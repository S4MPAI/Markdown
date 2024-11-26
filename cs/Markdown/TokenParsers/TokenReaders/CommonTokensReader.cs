using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenReaders;

public class CommonTokensReader : ITokenReader
{
    public IEnumerable<Token> ReadTokens(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            var currentChar = text.GetChar(i);
            var tokenType = GetTokenType(currentChar);

            if (tokenType != TokenType.Tag && tokenType != TokenType.Word)
                yield return Token.TryCreateCommonToken(currentChar)!.Value;
            
            yield return CreateTokenAndMovePointer(text, tokenType, ref i);
        }
    }

    private static TokenType GetTokenType(string content)
    {
        var token = Token.TryCreateCommonToken(content);

        if (token != null)
            return token.Value.Type;
        
        var isTag = Token.IsTagStartPart(content);
        
        return isTag ? TokenType.Tag : TokenType.Word;
    }
    
    private static Token CreateTokenAndMovePointer(string text, TokenType tokenType, ref int i)
    {
        var content = "";

        while (i < text.Length && !IsTokenEnded(text.GetChar(i), content, tokenType))
        {
            content += text[i];
            i++;
        }

        i--;

        return tokenType == TokenType.Tag
            ? Token.TryCreateTagToken(content, out _)!.Value
            : Token.CreateWordToken(content);
    }

    private static bool IsTokenEnded(string nextChar, string content, TokenType tokenType)
    {
        if (Token.TryCreateCommonToken(nextChar) != null || Token.IsTagStartPart(content))
            return true;
        return Token.TryCreateTagToken(content, out _) != null;
    }
}