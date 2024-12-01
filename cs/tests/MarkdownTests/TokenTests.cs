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
    public void CreateTextToken_ShouldReturnWordToken(string word)
    {
        var actualResult = Token.CreateTextToken(word);

        AssertToken(actualResult, word, TokenType.Word);
    }

    [TestCase(" ", TokenType.Space)]
    [TestCase("\n", TokenType.NewLine)]
    [TestCase("\r", TokenType.NewLine)]
    [TestCase("\\", TokenType.Escape)]
    public void TryCreateCommonToken_ShouldReturnExpectedToken(string content, TokenType expectedType)
    {
        Token.TryCreateCommonToken(content, out var actualToken).Should().BeTrue();
        AssertToken(actualToken, content, expectedType);
    }

    [TestCase("_")]
    [TestCase("# ")]
    [TestCase("__")]
    public void TryCreateTagToken_ShouldReturnExpectedToken(string content)
    {
        Token.TryCreateTagToken(content, out var actualToken).Should().BeTrue();
        AssertToken(actualToken, content, TokenType.Tag);
    }

    [TestCase("\\")]
    [TestCase(",")]
    [TestCase(".")]
    [TestCase(";")]
    [TestCase("/")]
    public void TryCreateSeparatorToken_ShouldReturnSeparatorToken(string content)
    {
        Token.TryCreateSeparatorToken(content, out var actualToken).Should().BeTrue();
        AssertToken(actualToken, content, TokenType.Separator);
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