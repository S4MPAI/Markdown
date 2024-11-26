using Markdown.TokenParsers.TokenHandlers;
using Markdown.TokenParsers.TokenReaders;
using Markdown.Tokens;

namespace Markdown.TokenParsers;

public class MarkdownTokenParser
{
    private readonly CommonTokensReader reader = new();
    private readonly List<ITokenHandler> tokenHandlers = CreateHandlers();

    private static List<ITokenHandler> CreateHandlers()
    {
        return new List<ITokenHandler>
            { 
                new EscapeTokenApplyHandler(), 
                new UnderscoreInNumberHandler() 
            }
            .OrderBy(x => x.Priority)
            .ToList();
    }

    public IEnumerable<Token> Parse(string text)
    {
        IList<Token> tokens = reader.ReadTokens(text).ToList();
        
        foreach (var tokenHandler in tokenHandlers)
            tokens = tokenHandler.Handle(tokens);
        
        return tokens;
    }
}