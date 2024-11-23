using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class EscapeTokenApplyHandler : ITokenHandler
{
    public int Priority => 1;

    public IList<Token> Handle(IList<Token> tokens)
    {
        var handledTokens = new List<Token>();
        
        for (var i = 0; i < tokens.Count; i++)
        {
            Token nextToken;
            
            if (tokens[i].Type != TokenType.Escape)
            {
                nextToken = tokens[i];
            }
            else if (i == tokens.Count - 1 || 
                tokens[i + 1].Type != TokenType.Escape ||
                tokens[i + 1].Type != TokenType.TagPart)
            {
                nextToken = Token.CreateWordToken(tokens[i].Content);
            }
            else if (tokens[i + 1].Type == TokenType.Escape)
            {
                nextToken = Token.CreateWordToken(tokens[i + 1].Content);
                i++;
            }
            else
            {
                nextToken = Token.CreateWordToken(tokens[i].Content + tokens[i + 1].Content);
                i++;
            }
            
            handledTokens.Add(nextToken);
        }
        
        return handledTokens;
    }
}