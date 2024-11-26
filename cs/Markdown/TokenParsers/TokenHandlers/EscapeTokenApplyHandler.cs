using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class EscapeTokenApplyHandler : ITokenHandler
{
    public int Priority => 1;

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = new List<Token>();
        Token? previousToken = null;
        
        foreach (var token in tokens)
        {
            if (previousToken?.Type == TokenType.Escape)
            {
                if (token.Type is not (TokenType.Tag or TokenType.Escape))
                    handledTokens.Add(Token.CreateWordToken(previousToken.Value.Content));

                handledTokens.Add(Token.CreateWordToken(token.Content));
                previousToken = null;
            }
            else if (token.Type == TokenType.Escape)
            {
                previousToken = token;
            }
            else
            {
                handledTokens.Add(token);
                previousToken = token;
            }
        }
        
        return handledTokens;
    }
}