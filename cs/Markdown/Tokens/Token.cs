namespace Markdown.Tokens;

public struct Token
{
    public TagType? Tag { get; }
    public TokenType Type { get; }
    public string Content { get; }

    private Token(string content, TokenType tokenType, TagType? tag = null)
    {
        Content = content;
        Type = tokenType;
        Tag = tag;
    }

    public static Token CreateWordToken(string content) => 
        new(content, TokenType.Word);

    public static Token? TryCreateCommonToken(string content)
    {
        if (int.TryParse(content, out _))
            return new Token(content, TokenType.Digit);

        return content switch
        {
            " " => new Token(content, TokenType.Space),
            "\n" => new Token(content, TokenType.NewLine),
            "\r" => new Token(content, TokenType.NewLine),
            "\\" => new Token(content, TokenType.Escape),
            "_" => new Token(content, TokenType.TagPart),
            "#" => new Token(content, TokenType.TagPart),
            _ => null
        };
    }
    
    public static Token? TryCreateTagToken(string content)
    {
        return content switch
        {
            "_" => new Token(content, TokenType.Tag, TagType.Italic),
            "__" => new Token(content, TokenType.Tag, TagType.Strong),
            "#" => new Token(content, TokenType.Tag, TagType.Header),
            _ => null
        };
    }
}