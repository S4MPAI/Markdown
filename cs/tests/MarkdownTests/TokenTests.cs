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
    [TestCase("_", TokenType.TagPart)]
    [TestCase("#", TokenType.TagPart)]
    public void TryCreateCommonToken_ShouldReturnExpectedToken(string content, TokenType expectedType)
    {
        var actualResult = Token.TryCreateCommonToken(content);

        actualResult.Should().NotBeNull();
        AssertToken(actualResult.Value, content, expectedType);
    }
    
    [Test]
    public void TryCreateCommonToken_ShouldReturnNull_NotCommonContent()
    {
        var actualResult = Token.TryCreateCommonToken("word");
        
        actualResult.Should().BeNull();
    }

    [TestCase("_", TagType.Italic)]
    public void TryCreateTagToken_ShouldReturnExpectedToken(string content, TagType expectedTag)
    {
        
    }
    
    private static void AssertToken(
        Token actualResult, 
        string expectedContent, 
        TokenType expectedType, 
        TagType? expectedTag = null)
    {
        using (new AssertionScope())
        {
            actualResult.Content.Should().Be(expectedContent);
            actualResult.Type.Should().Be(expectedType);
            actualResult.Tag.Should().Be(expectedTag);
        }
    }
}