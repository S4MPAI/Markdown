using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class NonPairTagTokensHandler : ITokenHandler
{
    public int Priority => 3;
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        throw new NotImplementedException();
    }
}