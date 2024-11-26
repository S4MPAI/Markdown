using Markdown.Tokens;

namespace Markdown.Tags;

public interface ITag
{
    public TagType TagType { get; }
    public bool IsCorrectOpenTag(IReadOnlyList<Token> tokens, int tokenPosition);
    public bool IsCorrectCloseTag(IReadOnlyList<Token> tokens, int tokenPosition);
    public bool IsOpenTag(Token token);
    public bool IsCloseTag(Token token);
    public bool IsTag(string content);
    public bool IsStartOfTag(string content);
    public bool IsCorrectContentInTag(IReadOnlyList<Token> tokens, int openTagPosition, int closeTagPosition);
}