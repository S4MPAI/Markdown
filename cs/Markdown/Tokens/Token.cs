using Markdown.Tags;

namespace Markdown.Tokens;

public struct Token : IEquatable<Token>
{
    public static readonly Token EndOfText = new("", TokenType.NewLine);
    private static readonly List<ITag> Tags =
    [
        new HeaderTag(),
        new ItalicTag(),
        new StrongTag()
    ];

    private static readonly HashSet<char> SeparatorSymbols = ['\\', ',', '.', ';', '/'];
    
    public readonly TokenType Type;
    public readonly string Content;

    private Token(string content, TokenType tokenType)
    {
        Content = content;
        Type = tokenType;
    }

    public static Token CreateTextToken(string content) =>
        new(content, TokenType.Word);

    public static bool TryCreateSeparatorToken(string content, out Token token)
    {
        token = default;
        if (!content.All(SeparatorSymbols.Contains)) 
            return false;
        
        token = new Token(content, TokenType.Separator);
        return true;
    }
        
    
    public static bool TryCreateCommonToken(string content, out Token token)
    {
        if (int.TryParse(content, out _))
        {
            token = new Token(content, TokenType.Digit);
            return true;
        }

        token = content switch
        {
            " " => new Token(content, TokenType.Space),
            "\n" or "\r" => new Token(content, TokenType.NewLine),
            "\\" => new Token(content, TokenType.Escape),
            _ => default
        };
        
        return !token.Equals(default);
    }
    
    public static bool TryCreateTagToken(string content, out Token token)
    {
        token = default;
        var neededTag = Tags.FirstOrDefault(tag => tag.IsTag(content));

        if (neededTag == null) 
            return false;
        
        token = new Token(content, TokenType.Tag);
        return true;
    }

    public static bool IsTagToken(Token token, TagType tagType) =>
        (TryGetTagTypeByOpenTag(token, out var openTag) && openTag == tagType) ||
        (TryGetTagTypeByCloseTag(token, out var closeTag) && closeTag == tagType);

    public static bool IsTagStartPart(string content) =>
        Tags.Any(tag => tag.IsStartOfTag(content));

    public static bool TryGetTagTypeByOpenTag(Token token, out TagType tagType)
    {
        tagType = default;
        if (!TryGetTagByOpenTag(token, out var tag)) 
            return false;
        
        tagType = tag!.TagType;
        return true;
    }

    public static bool TryGetTagByOpenTag(Token token, out ITag? tag)
    {
        tag = Tags.FirstOrDefault(x => x.IsOpenTag(token));
        return tag != null;
    }

    public static bool TryGetTagTypeByCloseTag(Token token, out TagType tagType)
    {
        tagType = default;
        if (!TryGetTagByCloseTag(token, out var tag))
            return false;
        tagType = tag!.TagType;
        return true;
    }

    public static bool TryGetTagByCloseTag(Token token, out ITag? tag)
    {
        tag = Tags.FirstOrDefault(x => x.IsCloseTag(token));
        return tag != null;
    }

    public bool Equals(Token other) => 
        Type == other.Type && Content == other.Content;

    public override bool Equals(object? obj) => 
        obj is Token other && Equals(other);

    public override int GetHashCode() => 
        HashCode.Combine((int)Type, Content);
}