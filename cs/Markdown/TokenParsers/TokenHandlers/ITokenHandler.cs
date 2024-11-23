using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public interface ITokenHandler
{
    public int Priority { get; }

    public IList<Token> Handle(IList<Token> tokens);
}