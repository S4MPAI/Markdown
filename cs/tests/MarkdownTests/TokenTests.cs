using FluentAssertions;
using FluentAssertions.Execution;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests;

public class TokenTests
{
    [TestCase("word")]
    [TestCase("///'][.,m1234/")]
    [TestCase("firstWord secondWord")]
    public void CreateWordToken_ShouldReturnWordToken(string word)
    {
        var actualResult = Token.CreateWordToken(word);

        AssertToken(actualResult, word, TokenType.Word);
    }

    [TestCase(" ", TokenType.Space)]
    [TestCase("\n", TokenType.NewLine)]
    [TestCase("\r", TokenType.NewLine)]
    [TestCase("\\", TokenType.Escape)]
    [TestCase("_", TokenType.Tag)]
    [TestCase("#", TokenType.Tag)]
    public void TryCreateCommonToken_ShouldReturnExpectedToken(string content, TokenType expectedType)
    {
        Token.TryCreateCommonToken(content, out var actualToken).Should().BeTrue();
        AssertToken(actualToken, content, expectedType);
    }

    [TestCase("_", TagType.Italic)]
    public void TryCreateTagToken_ShouldReturnExpectedToken(string content, TagType expectedTag)
    {
        
    }
    
    private static void AssertToken(
        Token actualResult, 
        string expectedContent, 
        TokenType expectedType)
    {
        using (new AssertionScope())
        {
            actualResult.Content.Should().Be(expectedContent);
            actualResult.Type.Should().Be(expectedType);
        }
    }
}