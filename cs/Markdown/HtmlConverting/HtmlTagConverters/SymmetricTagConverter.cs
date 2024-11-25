using Markdown.Tags;
using Markdown.Tokens;
using Markdown.Extensions;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public abstract class SymmetricTextTagConverter : IHtmlTagConverter
{
    protected abstract TagType NeededTag { get; }
    
    public IList<Token> ConvertToHtml(IReadOnlyList<Token> tokens)
    {
        var convertedTokens = new List<Token>();
        var tokenQueue = new Queue<Token>(tokens);
        
        for (var i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type != TokenType.Tag || tokens[i].Tag != NeededTag)
            {
                convertedTokens.Add(tokenQueue.Dequeue());
                continue;
            }

            var left = i++;
            for (; i < tokens.Count; i++)
                if (tokens[i].Type == TokenType.Tag && tokens[i].Tag == NeededTag)
                    break;

            if (i >= tokens.Count)
                convertedTokens.AddRange(tokenQueue.Dequeue(i - left));
            else
            {
                var htmlTag = HtmlTagsCreator.TryConvertMdTag(tokenQueue.Dequeue().Content);
                convertedTokens.Add(Token.CreateWordToken(HtmlTagsCreator.CreateOpenTag(htmlTag)));
                convertedTokens.AddRange(tokenQueue.Dequeue(i - left - 1));
                tokenQueue.Dequeue();
                convertedTokens.Add(Token.CreateWordToken(HtmlTagsCreator.CreateCloseTag(htmlTag)));
            }
        }
        
        return convertedTokens;
    }
}