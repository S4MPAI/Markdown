using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public interface ITokenHandler
{
    public int Priority { get; }

    public IEnumerable<Token> Handle(IEnumerable<Token> tokens);
}