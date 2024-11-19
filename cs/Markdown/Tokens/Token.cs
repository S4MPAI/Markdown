namespace Markdown.Tokens;

public struct Token(string text, TokenType tokenType)
{
    public TokenType Type { get; } = tokenType;
    public string Text { get; } = text;

    public Token(char text, TokenType tokenType) : this(text.ToString(), tokenType)
    {
    }
}