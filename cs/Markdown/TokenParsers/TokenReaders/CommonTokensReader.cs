using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenReaders;

public class CommonTokensReader : ITokenReader
{
    public IEnumerable<Token> ReadTokens(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            var currentChar = text.GetSymbol(i);

            if (Token.TryCreateCommonToken(currentChar, out var commonToken))
                yield return commonToken;
            else if (Token.TryCreateSeparatorToken(currentChar, out var punctuationToken))
                yield return punctuationToken;
            else
            {
                var tokenType = Token.IsTagStartPart(currentChar) ? TokenType.Tag : TokenType.Word;
                yield return CreateTokenAndMovePointer(text, tokenType, ref i);
            }
        }

        if (!Token.TryCreateCommonToken(text.GetSymbol(text.Length - 1), out var lastToken) ||
            lastToken.Type != TokenType.NewLine)
        {
            Token.TryCreateCommonToken("\n", out var lastNewLineToken);
            yield return lastNewLineToken;
        }
    }
    
    private static Token CreateTokenAndMovePointer(string text, TokenType tokenType, ref int i)
    {
        var content = "";

        while (i < text.Length && !IsTokenEnded(text.GetSymbol(i), content, tokenType))
        {
            content += text[i];
            i++;
        }

        i--;

        if (tokenType == TokenType.Tag && Token.TryCreateTagToken(content, out var token))
            return token;

        return Token.CreateTextToken(content);
    }

    private static bool IsTokenEnded(string nextChar, string content, TokenType tokenType)
    {
        if (tokenType == TokenType.Word &&
            (Token.TryCreateCommonToken(nextChar, out _) || Token.IsTagStartPart(nextChar)))
            return true;
        return Token.TryCreateTagToken(content, out _);
    }
}