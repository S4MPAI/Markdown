using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class UnderscoreTagTokensCreatingHandler : ITokenHandler
{
    public int Priority => 3;
    private const string TagContent = "_";

    public IList<Token> Handle(IList<Token> tokens)
    {
        var handledTokens = new List<Token>();
        
        for (var i = 1; i <= tokens.Count; i++)
        {
            if (i == tokens.Count || IsNotUnderscoreTagPart(tokens[i - 1]) || IsNotUnderscoreTagPart(tokens[i]))
            {
                var token = IsNotUnderscoreTagPart(tokens[i - 1]) ?
                    tokens[i - 1] :
                    Token.TryCreateTagToken(tokens[i - 1].Content) ?? tokens[i - 1];
                handledTokens.Add(token);
            }
            else
            {
                var token = Token.TryCreateTagToken(tokens[i - 1].Content + tokens[i].Content);
                if (token != null)
                    handledTokens.Add(token.Value);
                else
                {
                    handledTokens.Add(tokens[i - 1]);
                    handledTokens.Add(tokens[i]);
                }

                i++;
            }
            
        }
        
        return handledTokens;
    }

    private static bool IsNotUnderscoreTagPart(Token token) =>
        token.Type != TokenType.TagPart || token.Content != TagContent;
}