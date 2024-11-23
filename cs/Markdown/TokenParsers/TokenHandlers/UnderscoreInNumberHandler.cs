using System.Text;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class UnderscoreInNumberHandler : ITokenHandler
{
    public int Priority => 2;

    public IList<Token> Handle(IList<Token> tokens)
    {
        var handledTokens = new List<Token> { tokens[0] };

        for (var i = 1; i < tokens.Count - 1; i++)
        {
            if (tokens[i].Content != "_" || tokens[i - 1].Type != TokenType.Digit)
            {
                handledTokens.Add(tokens[i]);
            }
            else
            {
                var left = i;
                
                while (i < tokens.Count - 1 && tokens[i + 1].Content == "_")
                    i++;

                var length = i - left + 1;
                if (tokens[i + 1].Type == TokenType.Digit)
                {
                    var newToken = CombineTokensInOne(tokens, left, length);
                    handledTokens.Add(newToken);
                }
                else
                {
                    handledTokens.AddRange(tokens.Skip(left).Take(length));
                }
            }
        }
        
        handledTokens.Add(tokens[^1]);
        
        return handledTokens;
    }

    private static Token CombineTokensInOne(IList<Token> tokens, int start, int length)
    {
        var stringBuilder = new StringBuilder();

        for (var i = start; i < start + length; i++)
            stringBuilder.Append(tokens[i].Content);
        
        return Token.CreateWordToken(stringBuilder.ToString());
    }
}