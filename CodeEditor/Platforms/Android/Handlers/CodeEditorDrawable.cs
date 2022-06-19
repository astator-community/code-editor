using Android.Graphics;
using CodeEditor.Content;
using CodeEditor.Views.Droid;

namespace CodeEditor.Handlers.Droid;
internal class CodeEditorDrawable
{
    private readonly CodeEditorView editor;
    private Paints Paints => this.editor.Paints;
    private Cursor Cursor => this.editor.Cursor;
    private ContentText Text => this.editor.Text;

    public CodeEditorDrawable(CodeEditorView editor)
    {
        this.editor = editor;
    }

    public void OnDraw(Canvas canvas, ScrollEventArgs e)
    {
        canvas.Save();
        canvas.ClipRect(e.ScrollX, e.ScrollY, e.ScrollX + this.editor.Width, e.ScrollY + this.editor.Height);
        this.DrawCursor(canvas, e);
        this.DrawLineNumber(canvas, e);
        canvas.Restore();
        this.DrawHighLightText(canvas, e);
        this.DrawScrollBar(canvas, e);
    }

    private void DrawLineNumber(Canvas canvas, ScrollEventArgs e)
    {
        var startRow = e.StartRow;
        var endRow = e.EndRow;
        var width = 25 + this.Paints.GetLineNumberWidth(endRow + 1);
        for (var row = startRow; row <= endRow; row++)
        {
            var y = this.Paints.GetRowBaseline(row);
            canvas.DrawText((row + 1).ToString(), e.ScrollX + width, y, this.Paints.LineNumber);
        }
    }

    private void DrawHighLightText(Canvas canvas, ScrollEventArgs e)
    {
        var startRow = e.StartRow;
        var endRow = e.EndRow;
        var lineNumberWidth = 55 + this.Paints.GetLineNumberWidth(endRow + 1);
        canvas.Save();
        canvas.ClipRect(e.ScrollX + lineNumberWidth, e.ScrollY, e.ScrollX + this.editor.Width, e.ScrollY + this.editor.Height);
        var spans = this.Text.GetClassifiedSpans(this.Text.CurrLines[startRow].Start, this.Text.CurrLines[endRow].End);
        if (spans is null)
        {
            return;
        }
        for (var row = startRow; row <= endRow; row++)
        {
            var (x, left, width) = this.GetVisibleArgs(row, e.ScrollX, lineNumberWidth);
            if (x is -1)
            {
                continue;
            }
            var textSpan = this.Text.GetLineTextSpan(row);
            var startPosition = left + textSpan.Start;
            var line = this.Text.CurrLines[row];
            var endPosition = startPosition + Math.Min(line.End - line.Start - left, width);
            x += lineNumberWidth;
            var y = this.Paints.GetRowBaseline(row);

            if (startPosition >= endPosition)
            {
                continue;
            }

            var currPosition = startPosition;
            foreach (var span in spans)
            {
                if ((span.TextSpan.Start <= startPosition && span.TextSpan.End > startPosition)
                    || (span.TextSpan.Start >= startPosition && span.TextSpan.Start < endPosition))
                {
                    var paint = this.Paints.GetPaint(span.ClassificationType);
                    var start = Math.Max(startPosition, span.TextSpan.Start);
                    var end = Math.Min(endPosition, span.TextSpan.End);

                    if (start > currPosition)
                    {
                        var text = this.Text.ToString(new TextSpan(currPosition, start - currPosition));
                        canvas.DrawText(text, x, y, this.Paints.Normal);
                        x += this.Paints.MeasureText(text);
                        currPosition = start;
                    }

                    if (start < end)
                    {
                        var text = this.Text.ToString(new TextSpan(start, end - start));
                        canvas.DrawText(text, x, y, paint);
                        x += this.Paints.MeasureText(text);
                        currPosition = end;
                    }
                }
                else if (span.TextSpan.Start >= endPosition)
                {
                    if (endPosition > currPosition)
                    {
                        var text = this.Text.ToString(new TextSpan(currPosition, endPosition - currPosition));
                        canvas.DrawText(text, x, y, this.Paints.Normal);
                        x += this.Paints.MeasureText(text);
                    }
                    break;
                }
            }
        }
        canvas.Restore();
    }

    private (float, int, int) GetVisibleArgs(int row, float startX, float lineNumberWidth)
    {
        var text = this.Text.GetLineText(row);
        if (string.IsNullOrEmpty(text))
        {
            return (-1, -1, -1);
        }
        float sx; int start;
        if (startX <= 100)
        {
            (sx, start) = (0, 0);
        }
        else
        {
            var temp = (int)(startX / this.Paints.ENWidth);
            if (temp / 2 > text.Length)
            {
                return (-1, -1, -1);
            }
            temp = Math.Min(temp + 1, text.Length);
            (sx, start) = this.GetVisibleArgs(text, startX, 0, temp);
        }
        if (sx is -1)
        {
            return (-1, -1, -1);
        }
        var visibleWidth = startX - sx + this.editor.Width - lineNumberWidth;
        var visibleText = text[start..];
        var (_, width) = this.GetVisibleArgs(visibleText, visibleWidth, 0, Math.Min(visibleText.Length, (int)(visibleWidth / this.Paints.ENWidth)));
        if (width is -1) width = visibleText.Length;
        return (sx, start, Math.Min(width + 2, visibleText.Length));
    }

    public (float, int) GetVisibleArgs(string text, float startX, int left, int right)
    {
        var maxX = this.Paints.MeasureText(text);
        if (maxX < startX)
        {
            return (-1, -1);
        }
        var center = right;
        while (left < right - 1)
        {
            var x = this.Paints.MeasureText(text, 0, center);
            if (x == startX)
            {
                return (x, center);
            }
            else if (x < startX)
            {
                if ((x + this.Paints.ZHWidth) >= startX)
                {
                    return (x, center);
                }
                else
                {
                    left = center;
                    center = ((right - left) / 2) + left;
                }
            }
            else if (x > startX)
            {
                right = center;
                center = ((right - left) / 2) + left;
            }
        }
        return (startX, left);
    }

    private void DrawScrollBar(Canvas canvas, ScrollEventArgs e)
    {
        if (this.editor.ScrollBarEnabled)
        {
            var bounds = this.editor.GetScrollBarBounds();
            canvas.DrawRect(bounds, this.Paints.Normal);
        }
    }

    private void DrawCursor(Canvas canvas, ScrollEventArgs e)
    {
        var sx = this.Cursor.X;
        var sy = this.Cursor.Y;
        var ex = sx + 2;
        var ey = sy + this.Paints.RowHeight;
        canvas.DrawRect(e.ScrollX, sy, e.ScrollX + this.editor.Width, ey, this.Paints.FocusLine);
        if (this.Cursor.CheckDraw(e.StartRow, e.EndRow))
        {
            var lineNumberWidth = 55 + this.Paints.GetLineNumberWidth(e.EndRow + 1);
            if (sx >= e.ScrollX + lineNumberWidth)
            {
                canvas.DrawRect(sx, sy, ex, ey, this.Paints.Normal);
            }
        }
    }
}
