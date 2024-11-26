using Markdown.Tags;

namespace Markdown.Tokens;

public struct Token
{
    private static readonly List<ITag> Tags = new()
    {
        new HeaderTag(),
        new ItalicTag(),
        new StrongTag()
    };

    public TokenType Type;
    public string Content;

    private Token(string content, TokenType tokenType)
    {
        Content = content;
        Type = tokenType;
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
            "\n" or "\r" => new Token(content, TokenType.NewLine),
            "\\" => new Token(content, TokenType.Escape),
            _ => null
        };
    }
    
    public static Token? TryCreateTagToken(string content, out TagType? tagType)
    {
        tagType = null;

        var neededTag = Tags.FirstOrDefault(tag => tag.IsTag(content));

        if (neededTag != null)
            return new Token(content, TokenType.Tag);
        return null;
    }

    public static bool IsTagStartPart(string content) =>
        Tags.Any(tag => tag.IsStartOfTag(content));

    public static TagType? GetTagTypeByOpenTag(Token token) =>
        Tags.FirstOrDefault(x => x.IsOpenTag(token))?.TagType;
    
    public static ITag? GetTagByOpenTag(Token token) => 
        Tags.FirstOrDefault(x => x.IsOpenTag(token));
}