using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Formatting;
using System.Text.RegularExpressions;
using Document = Microsoft.CodeAnalysis.Document;

namespace CodeEditor.Content;
public class ContentText
{
    public SourceText CurrText { get; private set; }
    public TextLineCollection CurrLines => this.CurrText.Lines;
    public int[] CurrLineStarts { get; private set; }
    public int CurrLineCount => this.CurrLines.Count;
    public int CurrEditPosition { get; set; }
    public Action<int> OnTextChanged { get; set; }

    private Document currDocument;
    private readonly CompletionService completionService;
    private readonly UndoManager undoManager = new();

    public ContentText(Document document, Action<int> onTextChanged)
    {
        this.currDocument = document;
        this.CurrText = document.GetTextAsync().Result;
        this.CurrLineStarts = this.GetLineStarts();
        this.completionService = CompletionService.GetService(this.currDocument);
        this.OnTextChanged = onTextChanged;
    }

    public TextSpan GetLineTextSpan(int row)
    {
        return this.CurrLines[row].Span;
    }

    public string GetLineText(int row)
    {
        return this.ToString(this.GetLineTextSpan(row));
    }

    public string ToString(TextSpan textSpan)
    {
        return this.CurrText.ToString(textSpan);
    }

    private int[] GetLineStarts()
    {
        var field = this.CurrText.Lines.GetType().GetField("_lineStarts", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (int[])field.GetValue(this.CurrText.Lines);
    }

    public void Undo()
    {
        var (change, editorPosition) = this.undoManager.GetUndoCache();
        if (editorPosition != -1)
        {
            this.CurrText = this.CurrText.WithChanges(change);
            this.CurrEditPosition = editorPosition;
            this.TextChanged(editorPosition);
        }
    }

    public void Redo()
    {
        var (change, editorPosition) = this.undoManager.GetRedoCache();
        if (editorPosition != -1)
        {
            this.CurrText = this.CurrText.WithChanges(change);
            this.CurrEditPosition = editorPosition;
            this.TextChanged(editorPosition);
        }
    }

    public void WithChanges(TextChange change, int position = -1)
    {
        var oldText = this.ToString(change.Span);
        var newText = change.NewText;
        var undoTextChange = new TextChange(new TextSpan(change.Span.Start, newText.Length), oldText);
        var oldPosition = this.CurrEditPosition;
        this.CurrEditPosition = position != -1 ? position : this.CurrEditPosition + newText.Length - change.Span.Length;
        this.undoManager.Add(undoTextChange, oldPosition, change, this.CurrEditPosition);
        this.CurrText = this.CurrText.WithChanges(change);
        this.TextChanged(this.CurrEditPosition);
    }

    public void TextChanged(int editPosition)
    {
        editPosition = Math.Min(editPosition, this.CurrText.Length - 1);
        this.currDocument = this.currDocument.WithText(this.CurrText);
        this.CurrLineStarts = this.GetLineStarts();
        this.OnTextChanged?.Invoke(editPosition);
    }

    public async Task FormatCode()
    {
        this.currDocument = await Formatter.FormatAsync(this.currDocument);
        this.CurrText = await this.currDocument.GetTextAsync();
        this.TextChanged(this.CurrEditPosition);
    }

    public void InsertSymbol(string value)
    {
        if (value is "{}" or "()" or "\"\"")
        {
            this.WithChanges(new TextChange(new TextSpan(this.CurrEditPosition, 0), value), this.CurrEditPosition + 1);
        }
        else
        {
            this.WithChanges(new TextChange(new TextSpan(this.CurrEditPosition, 0), value));
        }
    }

    public void InsertEnter()
    {
        var row = this.CurrLines.IndexOf(this.CurrEditPosition);
        var start = this.CurrLines[row].Start;
        var end = this.CurrLines[row].End;

        var indent = this.GetLineIndent(row, end);
        var whiteSpaces = new char[indent];
        Array.Fill(whiteSpaces, ' ');
        var whiteSpaceText = new string(whiteSpaces);

        var left = -1;
        var right = -1;
        for (int i = this.CurrEditPosition - 1; i >= start; i--)
        {
            var c = this.CurrText[i];
            if (!char.IsWhiteSpace(c))
            {
                if (c is '{')
                {
                    left = i;
                }
                break;
            }
        }

        for (int i = this.CurrEditPosition; i <= end; i++)
        {
            var c = this.CurrText[i];
            if (!char.IsWhiteSpace(c))
            {
                if (c is '}')
                {
                    right = i;
                }
                break;
            }
        }


        if (left != -1 && right != -1)
        {
            var InsertOnNewLine = false;
            for (int i = start; i < left; i++)
            {
                var c = this.CurrText[i];
                if (!char.IsWhiteSpace(c))
                {
                    if (c != '{')
                    {
                        InsertOnNewLine = true;
                    }
                    break;
                }
            }
            var first = InsertOnNewLine ? $"\n{whiteSpaceText}{{" : "{";
            var position = left + (InsertOnNewLine ? (whiteSpaces.Length * 2) + 7 : whiteSpaces.Length + 6);
            this.WithChanges(new TextChange(new TextSpan(left, right - left + 1), $"{first}\n{whiteSpaceText}    \n{whiteSpaceText}}}"), position);
        }
        else
        {
            this.WithChanges(new TextChange(new TextSpan(this.CurrEditPosition, 0), $"\n{whiteSpaceText}"));
        }
    }


    public void Insert(string text)
    {
        this.WithChanges(new TextChange(new TextSpan(this.CurrEditPosition, 0), text));
    }

    public void Replace(int position, int length, string text)
    {
        this.WithChanges(new TextChange(new TextSpan(position, length), text));
    }

    public void Remove(int position, int length)
    {
        this.WithChanges(new TextChange(new TextSpan(position, length), string.Empty));
    }

    /// <summary>
    /// 获取span
    /// </summary>
    public List<ClassifiedSpan> GetClassifiedSpans(int startPosition, int endPosition)
    {
        var spans = Classifier.GetClassifiedSpansAsync(this.currDocument,
            TextSpan.FromBounds(startPosition, endPosition)).Result;
        if (spans.Any())
        {
            return spans.Where(span =>
             !ClassificationTypeNames.AdditiveTypeNames.Contains(span.ClassificationType)).
             ToList();
        }
        return default;
    }

    /// <summary>
    /// 获取完成项列表
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public List<CompletionItem> GetCompletions(int position)
    {
        try
        {
            var result = new List<CompletionItem>();
            var list = this.completionService.GetCompletionsAsync(this.currDocument, position).Result;
            if (list.Span.IsEmpty)
            {
                //当插入字符为'.'时返回全部item, 否则返回空
                if (this.CurrText[list.Span.Start - 1] is '.')
                    return list.Items.ToList();
                else
                    return result;
            }
            var text = this.CurrText.ToString(list.Span);
            var pattern = $"^[{char.ToUpper(text[0])}{char.ToLower(text[0])}]+";
            for (int i = 1; i < text.Length; i++)
            {
                var upper = char.ToUpper(text[i]);
                var lower = char.ToLower(text[i]);
                pattern += $"(/?([^{lower}]+)[{upper}]+|[{lower}{upper}]+)";
            }
            var regex = new Regex(pattern);

            foreach (var item in list.Items)
            {
                if (regex.IsMatch(item.DisplayText))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        catch (Exception)
        {
            return default;
        }
    }

    /// <summary>
    /// 获取完成项的描述
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public string GetCompletionItemDescription(CompletionItem item)
    {
        return this.completionService.GetDescriptionAsync(this.currDocument, item).Result.Text;
    }

    /// <summary>
    /// 执行自动完成
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void ExecuteAutoCompletion(CompletionItem item)
    {
        var change = this.completionService.GetChangeAsync(this.currDocument, item).Result.TextChange;
        this.WithChanges(change);

        var newPosition = item.Span.Start + change.NewText.Length;
        if (item.Properties.ContainsKey("ShouldProvideParenthesisCompletion"))
        {
            if (bool.Parse(item.Properties["ShouldProvideParenthesisCompletion"]))
            {
                this.WithChanges(new TextChange(new TextSpan(newPosition, 0), "()"), newPosition + 1);
            }
        }
    }

    /// <summary>
    /// 获取行缩进
    /// </summary>
    /// <param name="row"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public int GetLineIndent(int row, int position)
    {
        if (row > 0 && row < this.CurrLines.Count)
        {
            var count = 0;
            var record = true;
            for (int i = this.CurrLineStarts[row]; i < position; i++)
            {
                var c = this.CurrText[i];
                if (record && char.IsWhiteSpace(c))
                    count++;
                else
                    record = false;

                if (c is '{')
                    count += 4;
                else if (c is '}')
                    count -= 4;
            }
            return Math.Max(count, 0);
        }
        return 0;
    }

}
