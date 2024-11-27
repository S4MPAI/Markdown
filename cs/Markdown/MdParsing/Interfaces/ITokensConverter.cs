using Markdown.Tokens;

namespace Markdown.MdParsing.Interfaces;

public interface ITokensConverter
{
    public string Convert(IEnumerable<Token> tokens);
}