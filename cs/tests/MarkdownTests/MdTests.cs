using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MdTests
{
    [TestCaseSource(nameof(RenderTestCases))]
    public string Render_ShouldReturnExpectedResult(string text)
    {
        var md = new Md();
        
        return md.Render(text);
    }

    private static readonly object[] RenderTestCases =
    {
        new TestCaseData("Текст, _окруженный с двух сторон_ одинарными символами подчерка")
            .Returns("Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка")
            .SetName("ContainsItalic"),
        new TestCaseData("__Выделенный двумя символами текст__ должен становиться полужирным")
            .Returns("<strong>Выделенный двумя символами текст</strong> должен становиться полужирным")
            .SetName("ContainsStrong"),
        new TestCaseData(@"\_Вот это\_, не должно выделиться тегом <em>.")
            .Returns("_Вот это_, не должно выделиться тегом <em>.")
            .SetName("ContainsEscapeNearTag"),
        new TestCaseData(@"Здесь сим\волы экранирования\ \должны остаться.\")
            .Returns(@"Здесь сим\волы экранирования\ \должны остаться.\")
            .SetName("ContainsEscapeWithoutTag"),
        new TestCaseData(@"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_")
            .Returns(@"Символ экранирования тоже можно экранировать: \<em>вот это будет выделено тегом</em>")
            .SetName("ContainsEscapeNearEscape"),
        new TestCaseData("Внутри __двойного выделения _одинарное_ тоже__ работает.")
            .Returns("Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")
            .SetName("ContainsItalicInStrong"),
        new TestCaseData("внутри _одинарного __двойное__ не_ работает.")
            .Returns("внутри _одинарного __двойное__ не_ работает.")
            .SetName("ContainsStrongInItalic"),
        new TestCaseData("Подчерки внутри текста c цифрами_12_3 не считаются выделением")
            .Returns("Подчерки внутри текста c цифрами_12_3 не считаются выделением")
            .SetName("ContainsItalicInTextWithNumbers"),
        new TestCaseData("выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._")
            .Returns("выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>")
            .SetName("ContainsItalicInWord"),
        new TestCaseData("В то же время выделение в ра_зных сл_овах не работает.")
            .Returns("В то же время выделение в ра_зных сл_овах не работает.")
            .SetName("ContainsItalicInDifferentWords"),
        new TestCaseData("__Непарные_ символы в рамках одного абзаца не считаются выделением.")
            .Returns("__Непарные_ символы в рамках одного абзаца не считаются выделением.")
            .SetName("ContainsUnpairedTags"),
        new TestCaseData("эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.")
            .Returns("эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.")
            .SetName("ContainsSpaceAfterOpenItalicTag"),
        new TestCaseData("эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.")
            .Returns("эти <em>подчерки _не считаются</em> окончанием выделения и остаются просто символами подчерка.")
            .SetName("ContainsSpaceBeforeCloseItalicTag"),
        new TestCaseData("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.")
            .Returns("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.")
            .SetName("DifferentTagsIntersects"),
        new TestCaseData("Если внутри подчерков пустая строка ____, то они остаются символами подчерка.")
            .Returns("Если внутри подчерков пустая строка ____, то они остаются символами подчерка.")
            .SetName("TagsContainsEmptyString"),
        new TestCaseData("# Заголовок __с _разными_ символами__")
            .Returns("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
            .SetName("HeaderWithOtherTags"),
        new TestCaseData("_Теги в разных абзацах\nне должны считаться_ выделением")
            .Returns("_Теги в разных абзацах\nне должны считаться_ выделением")
            .SetName("TagsInDifferentParagraphs"),
    };
}