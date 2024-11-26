using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class TagTokensIntersectsHandler : ITokenHandler
{
    public int Priority => 4;

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        throw new NotImplementedException();
    }
}