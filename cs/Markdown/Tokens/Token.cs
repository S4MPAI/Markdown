namespace Markdown.Tokens;

public struct Token
{
    private static readonly Dictionary<string, TagType> Tags = new()
    {
        {"_", TagType.Italic },
        {"__", TagType.Strong },
        {"# ", TagType.Header }
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
    
    public static Token? TryCreateTagToken(string content, out TagType? tag)
    {
        tag = null;
        
        if (!Tags.TryGetValue(content, out var tagType)) 
            return null;
        
        tag = tagType;
        return new Token(content, TokenType.Tag);

    }

    public static TagType? GetTagType(Token token)
    {
        Tags.TryGetValue(token.Content, out var tagType);
        
        return tagType;
    }

    public static bool IsTagStartPart(string content) =>
        Tags.Any(tag => tag.Key.StartsWith(content));

    public static bool IsValidTokenTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var currentToken = tokens[tokenPosition];
        if (currentToken.Type != TokenType.Tag || !Tags.TryGetValue(currentToken.Content, out var tagType))
            return false;

        return tagType switch
        {
            TagType.Italic or TagType.Strong => IsValidStrongOrItalicTag(tokens, tokenPosition),
            TagType.Header => IsValidHeaderTag(tokens, tokenPosition),
            _ => false
        };
    }
    
    public static bool IsValidStrongOrItalicTag(IReadOnlyList<Token> tokens, int tokenPosition) => 
        IsValidStrongOrItalicOpenTag(tokens, tokenPosition) || IsValidStrongOrItalicCloseTag(tokens, tokenPosition);

    public static bool IsValidStrongOrItalicOpenTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        CheckTokenInRange(tokens, tokenPosition);
        var nextPosition = tokenPosition + 1;
        
        return nextPosition < tokens.Count && tokens[nextPosition].Type != TokenType.Space;
    }
    
    public static bool IsValidStrongOrItalicCloseTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        CheckTokenInRange(tokens, tokenPosition);
        var previousPosition = tokenPosition - 1;
        
        return previousPosition >= 0 && tokens[previousPosition].Type != TokenType.Space;
    }
    
    public static bool IsValidHeaderTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        CheckTokenInRange(tokens, tokenPosition);
        var previousPosition = tokenPosition - 1;
        var nextPosition = tokenPosition + 1;
        
        return (tokenPosition == 0 || tokens[previousPosition].Type == TokenType.NewLine) && 
               nextPosition < tokens.Count && 
               tokens[nextPosition].Type == TokenType.Space;
    }

    private static void CheckTokenInRange(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        if (tokenPosition >= tokens.Count || tokenPosition < 0)
            throw new ArgumentException("Invalid token position", nameof(tokenPosition));
    }
}