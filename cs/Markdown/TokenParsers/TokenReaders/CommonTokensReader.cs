using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenReaders;

public class CommonTokensReader : ITokenReader
{
    public IEnumerable<Token> ReadTokens(string text)
    {
        var left = 0;
        
        for (var i = 0; i < text.Length; i++)
        {
            var currentChar = text.Substring(i, 1);
            
            var nextToken = Token.TryCreateCommonToken(currentChar);

            if (nextToken == null) 
                continue;
            
            var wordLength = i - left;
            if (wordLength > 0)
                yield return Token.CreateWordToken(text.Substring(left, wordLength));

            left = i + 1;
                
            yield return nextToken.Value;
        }

        if (left < text.Length)
            yield return Token.CreateWordToken(text.Substring(left, text.Length - left));
    }
}