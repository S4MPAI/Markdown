using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class UnderscoreInNumberHandler : ITokenHandler
{
    public int Priority => 2;

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var tokenQueue = new Queue<Token>(tokens);
        var handledTokens = new List<Token> { tokenQueue.Dequeue() };
        Token? previousToken = tokens[0];

        while (tokenQueue.Count > 0)
        {
            Token? currentToken = tokenQueue.Peek();
            var tagType = Token.GetTagTypeByOpenTag(currentToken.Value);
            if (tagType is not TagType.Italic and not TagType.Strong && previousToken is not { Type: TokenType.Digit })
            {
                handledTokens.Add(tokenQueue.Dequeue());
            }
            else
            {
                var (token, length) = MoveToTokenWithAnotherTag(tokenQueue);

                if (token?.Type == TokenType.Digit)
                {
                    var newToken = CombineTokensInOne(tokenQueue, length);
                    handledTokens.Add(newToken);
                }
                else
                    handledTokens.AddRange(tokenQueue.Dequeue(length));
                currentToken = null;
            }
            
            previousToken = currentToken;
        }
        
        handledTokens.Add(tokens[^1]);
        
        return handledTokens;
    }

    private static (Token?, int position) MoveToTokenWithAnotherTag(IEnumerable<Token> tokens)
    {
        var i = 0;
        
        foreach (var token in tokens)
        {
            var tagType = Token.GetTagTypeByOpenTag(token);

            if (token.Type != TokenType.Tag && tagType is not TagType.Italic and not TagType.Strong)
                return (token, i);
                
            i++;
        }

        return (null, -1);
    }
    
    private static Token CombineTokensInOne(Queue<Token> tokens, int length)
    {
        var stringBuilder = new StringBuilder();

        var tokensContent = tokens.Dequeue(length).Select(token => token.Content);
        stringBuilder.Append(tokensContent);
        
        return Token.CreateWordToken(stringBuilder.ToString());
    }
}