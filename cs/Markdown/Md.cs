using Markdown.HtmlConverting;
using Markdown.TokenParsers;
using Markdown.TokenParsers.TokenHandlers;
using Markdown.TokenParsers.TokenReaders;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    private readonly IMarkdownConverter converter = new MarkdownToHtmlConverter();
    private readonly CommonTokensReader reader = new();
    private readonly List<ITokenHandler> tokenHandlers = CreateHandlers();

    private static List<ITokenHandler> CreateHandlers()
    {
        return new List<ITokenHandler>
            { 
                new EscapeTokenApplyHandler(), 
                new IncorrectTagTokensHandler(),
            }
            .OrderBy(x => x.Priority)
            .ToList();
    }

    public string Render(string text)
    {
        IReadOnlyList<Token> tokens = reader.ReadTokens(text).ToList();
        
        foreach (var tokenHandler in tokenHandlers)
            tokens = tokenHandler.Handle(tokens);
        
        return converter.Convert(tokens);
    }
}