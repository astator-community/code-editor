using Android.Graphics;
using Android.Text;
using Android.Views.InputMethods;
using CodeEditor.Content;
using CodeEditor.Handlers.Droid;
using CodeEditor.Themes;
using static Android.Views.View;
using Cursor = CodeEditor.Handlers.Droid.Cursor;
using InputConnection = CodeEditor.Handlers.Droid.InputConnection;
using Rect = Android.Graphics.Rect;

namespace CodeEditor.Views.Droid;
public class CodeEditorView : View, IOnTouchListener
{
    internal ContentText Text { get; set; }
    internal Paints Paints { get; set; }
    internal Cursor Cursor { get; set; }
    internal bool ScrollBarEnabled { get; set; }
    internal AutoCompletionWindow CompletionWindow { get; init; }

    private DateTime scrollBarUpdateTime = DateTime.Now;

    private readonly GestureDetector gestureDetector;
    private readonly GestureListener gestureListener;
    private readonly InputConnection inputConnection;
    private readonly InputMethodManager inputMethodManager;

    private readonly CodeEditorDrawable drawable;

    public CodeEditorView(Context context, ITheme theme, int textSize) : base(context)
    {
        this.SetBackgroundColor(theme.Background);
        this.Paints = new Paints(theme, textSize);
        this.Cursor = new Cursor();
        this.drawable = new CodeEditorDrawable(this);
        this.gestureListener = new GestureListener(this);
        this.gestureDetector = new GestureDetector(this.Context, this.gestureListener);
        this.SetOnTouchListener(this);
        this.inputConnection = new InputConnection(this);
        this.inputMethodManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);

        this.CompletionWindow = new AutoCompletionWindow(context, this.OnCompletionShowed, this.OnCompletionItemClicked) { AnchorView = this };
    }

    public void SetDocument(Document document)
    {
        this.Text = new ContentText(document, this.OnTextChanged);
        this.Invalidate();
    }

    public void SetTheme(ITheme theme)
    {
        this.SetBackgroundColor(theme.Background);
        this.Paints.SetTheme(theme);
        this.Invalidate();
    }

    public void SetTextSize(int textSize)
    {
        this.Paints.SetTextSize(textSize);
        this.Invalidate();
    }

    public void Undo()
    {
        this.Text.Undo();
    }

    public void Redo()
    {
        this.Text.Redo();
    }

    public async void FormatCode()
    {
        await this.Text.FormatCode();
        this.Invalidate();
    }

    public void CommitSymbol(string value)
    {
        this.Text.InsertSymbol(value);
        if (value is ".")
        {
            this.ComplitionShow();
        }
    }

    private void OnTextChanged(int position)
    {
        this.UpdateCursor(position);
    }

    protected override void OnDraw(Canvas canvas)
    {
        if (this.Text is not null)
        {
            var e = new ScrollEventArgs(this.GetScrollX(), this.GetScrollY(), this.GetStartVisibleRow(), this.GetEndVisibleRow());
            this.drawable.OnDraw(canvas, e);
        }
    }

    protected override void OnAttachedToWindow()
    {
        base.OnAttachedToWindow();
        this.CompletionWindow.Width = this.Width;
        this.CheckCursorAndScrollBar();
    }

    private void CheckCursorAndScrollBar()
    {
        Task.Run(async () =>
        {
            while (this.IsAttachedToWindow)
            {
                await Task.Delay(500);
                var time = DateTime.Now;
                if ((DateTime.Now - this.Cursor.UpdateTime).TotalMilliseconds > 500)
                {
                    this.Cursor.UpdateTime = time;
                    this.Cursor.Showing = !this.Cursor.Showing;
                    var startRow = this.GetStartVisibleRow();
                    var endRow = this.GetEndVisibleRow();
                    if (this.Cursor.CurrRow >= startRow && this.Cursor.CurrRow <= endRow)
                    {
                        this.Invalidate();
                    }
                }
                if ((DateTime.Now - this.scrollBarUpdateTime).TotalMilliseconds >= 2000)
                {
                    this.ScrollBarEnabled = false;
                    this.Invalidate();
                }
            }
        });
    }

    public bool OnTouch(View v, MotionEvent e)
    {
        var result = this.gestureListener.OnTouchEvent(e);
        if (result) return true;
        return this.gestureDetector.OnTouchEvent(e);
    }

    public override bool OnCheckIsTextEditor()
    {
        return true;
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        if (MeasureSpec.GetMode(widthMeasureSpec) is MeasureSpecMode.AtMost or MeasureSpecMode.Unspecified)
        {
            widthMeasureSpec = MeasureSpec.MakeMeasureSpec(MeasureSpec.GetSize(widthMeasureSpec), MeasureSpecMode.Exactly);
        }
        if (MeasureSpec.GetMode(heightMeasureSpec) is MeasureSpecMode.AtMost or MeasureSpecMode.Unspecified)
        {
            heightMeasureSpec = MeasureSpec.MakeMeasureSpec(MeasureSpec.GetSize(heightMeasureSpec), MeasureSpecMode.Exactly);
        }
        base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
    }

    protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
    {
        if (h < oldh)
        {
            var y = this.Cursor.CurrRow * this.Paints.RowHeight;
            var dy = 0;
            if (y < this.ScrollY)
            {
                dy = y - this.ScrollY;
            }
            if (y > this.ScrollY + this.Height - this.Paints.RowHeight)
            {
                dy = y - this.ScrollY - this.Height + this.Paints.RowHeight;
            }
            if (dy != 0)
            {
                this.gestureListener.Scroller.StartScroll(this.ScrollX, this.ScrollY, 0, dy, 0);
                this.Invalidate();
            }
        }
    }

    public override IInputConnection OnCreateInputConnection(EditorInfo outAttrs)
    {
        outAttrs.InputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine;
        outAttrs.InitialSelStart = 0;
        outAttrs.InitialSelEnd = 0;
        outAttrs.InitialCapsMode = this.inputConnection.GetCursorCapsMode(0);
        return this.inputConnection;
    }

    public int GetMaxScrollX()
    {
        TextSpan maxSpan = new TextSpan(0, 0);
        for (var row = 0; row < this.Text.CurrLines.Count; row++)
        {
            if (this.Text.CurrLines[row].Span.Length > maxSpan.Length)
            {
                maxSpan = this.Text.CurrLines[row].Span;
            }
        }
        var value = (int)(this.Paints.MeasureText(this.Text.ToString(maxSpan)) - (this.Width / 2));
        return value > (this.Width / 2) ? value : 0;
    }

    public int GetMaxScrollY()
    {
        var value = (this.Text.CurrLineCount * this.Paints.RowHeight) - (this.Height / 2);
        return value > (this.Height / 2) ? value : 0;
    }

    public int GetScrollX() => this.ScrollX;

    public int GetScrollY() => this.ScrollY;

    public int GetStartVisibleRow() => Math.Max(0, this.GetScrollY() / this.Paints.RowHeight);

    public int GetEndVisibleRow() => Math.Min(this.Text.CurrLineCount - 1, (this.GetScrollY() + this.Height) / this.Paints.RowHeight);

    public override void ComputeScroll()
    {
        var scroller = this.gestureListener.Scroller;
        if (scroller.ComputeScrollOffset())
        {
            this.scrollBarUpdateTime = DateTime.Now;
            this.ScrollTo(scroller.CurrX, scroller.CurrY);
            this.Invalidate();
        }
    }

    public Rect GetScrollBarBounds()
    {
        var barWidth = Util.Dp2Px(12);
        var width = this.Width;
        var height = this.Height;
        var maxHeight = (float)this.GetMaxScrollY() + height;
        var barHeight = (int)(height / maxHeight * height);
        var offsetY = (int)(this.ScrollY / maxHeight * height);

        var ex = this.ScrollX + width;
        var sx = ex - barWidth;
        var sy = this.ScrollY + offsetY;
        var ey = sy + barHeight;
        return new Rect(sx, sy, ex, ey);
    }

    public void CommitText(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        this.Text.Insert(text);
        if (!string.IsNullOrWhiteSpace(text))
        {
            this.ComplitionShow();
        }
        else
        {
            this.CompletionWindow.Dismiss();
        }
    }

    public void ComplitionShow()
    {
        var completionItems = this.Text.GetCompletions(this.Cursor.Position);
        if (completionItems is not null && completionItems.Any())
        {
            var textColor = this.Paints.Theme.IsDarkMode ? Color.White : Color.Black;
            this.CompletionWindow.SetAdapter(completionItems, textColor, (item) =>
            {
                return this.Text.GetCompletionItemDescription(item);
            });
            this.CompletionWindow.Show();
        }
        else
        {
            this.CompletionWindow.Dismiss();
        }
    }

    public void OnCompletionShowed()
    {
        this.CompletionWindow.ListView.SetBackgroundColor(this.Paints.Theme.TitleBarBackground);
        var y = this.Cursor.CurrRow * this.Paints.RowHeight;
        var dy = 0;
        if (y > this.ScrollY + this.Height - this.Paints.RowHeight - this.CompletionWindow.Height)
        {
            dy = y - this.ScrollY - this.Height + this.Paints.RowHeight + this.CompletionWindow.Height;
        }
        if (dy != 0)
        {
            this.gestureListener.Scroller.StartScroll(this.ScrollX, this.ScrollY, 0, dy, 0);
            this.Invalidate();
        }
    }

    public void OnCompletionItemClicked(int position)
    {
        var item = this.CompletionWindow.GetItem(position);
        this.Text.ExecuteAutoCompletion(item);
        this.CompletionWindow.Dismiss();
    }

    public void UpdateCursor(int position)
    {
        var row = this.Text.CurrLines.IndexOf(position);
        var column = position - this.Text.CurrLineStarts[row];
        this.UpdateCursor(row, column, position);
    }

    public void UpdateCursor(int row, int column, int position = -1)
    {
        row = Math.Max(row, 0);
        row = Math.Min(row, this.Text.CurrLineCount - 1);
        var line = this.Text.CurrLines[row];
        column = Math.Min(column, line.End - line.Start);
        var lineNumberWidth = 55 + (int)this.Paints.GetLineNumberWidth(this.GetEndVisibleRow() + 1);
        var reserveWidth = lineNumberWidth + (int)this.Paints.ENWidth;
        position = position != -1 ? position : line.Start + column;

        var x = 0f;
        var y = row * this.Paints.RowHeight;
        var start = this.Text.CurrLines[row].Start;
        var end = Math.Min(position, this.Text.CurrLines[row].End);
        var offset = start;

        var spans = this.Text.GetClassifiedSpans(line.Start, line.End);
        if (spans is not null)
        {
            foreach (var span in spans)
            {
                var currStart = Math.Max(start, span.TextSpan.Start);
                var currEnd = Math.Min(end, span.TextSpan.End);

                if (currStart < currEnd)
                {
                    x += this.Paints.ENWidth * (currStart - offset);
                    x += this.Paints.MeasureText(this.Text.ToString(new TextSpan(currStart, currEnd - currStart)));
                }
                else
                {
                    x += this.Paints.ENWidth * (currEnd - offset);
                }
                offset = currEnd;
            }
        }
        x += this.Paints.ENWidth * (end - offset);


        this.Text.CurrEditPosition = position;
        var dx = 0;
        var dy = 0;

        if (x < this.ScrollX || y < this.ScrollY || x > this.ScrollX + this.Width || y > this.ScrollY + this.Height)
        {
            dx = (int)x - this.ScrollX;
        }
        if (x > this.ScrollX + this.Width - reserveWidth)
        {
            dx = (int)x - this.ScrollX - this.Width + reserveWidth;
        }
        if (y < this.ScrollY)
        {
            dy = y - this.ScrollY;
        }
        if (y > this.ScrollY + this.Height - this.Paints.RowHeight)
        {
            dy = y - this.ScrollY - this.Height + this.Paints.RowHeight;
        }

        if (dx != 0 || dy != 0)
        {
            this.gestureListener.Scroller.StartScroll(this.ScrollX, this.ScrollY, dx, dy, 0);
        }

        this.Cursor.Update(position, (int)x + lineNumberWidth, y, row, column);
        this.Invalidate();
    }

    public void OpenIME()
    {
        if (!this.Focusable || !this.FocusableInTouchMode)
        {
            this.Focusable = true;
            this.FocusableInTouchMode = true;
            this.RequestFocus();
            this.RequestFocusFromTouch();
        }
        this.inputMethodManager.ShowSoftInput(this, 0);
    }
}
