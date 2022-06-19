using Android.Widget;
using CodeEditor.Views.Droid;
using static Android.Views.GestureDetector;

namespace CodeEditor.Handlers.Droid;
internal class GestureListener : Java.Lang.Object, IOnGestureListener, IOnDoubleTapListener
{
    public OverScroller Scroller { get; init; }
    public bool IsOnFling { get; set; } = false;

    private readonly CodeEditorView editor;

    public GestureListener(CodeEditorView editor)
    {
        this.editor = editor;
        this.Scroller = new OverScroller(editor.Context);
    }

    public bool OnDoubleTap(MotionEvent e)
    {
        return true;
    }

    public bool OnDoubleTapEvent(MotionEvent e)
    {
        return true;
    }


    public bool OnDown(MotionEvent e)
    {
        return true;
    }


    public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
    {
        this.editor.ScrollBarEnabled = true;
        this.Scroller.ForceFinished(true);
        this.Scroller.Fling(this.Scroller.CurrX, this.Scroller.CurrY,
            (int)-velocityX, (int)-velocityY,
            0, this.editor.GetMaxScrollX(),
            0, this.editor.GetMaxScrollY(),
            0, 0);

        if (Math.Abs(velocityX) > 150) this.IsOnFling = true;
        this.editor.Invalidate();
        return true;

    }

    public void OnLongPress(MotionEvent e)
    {
    }

    public bool OnScale(ScaleGestureDetector detector)
    {
        return true;
    }

    public bool OnScaleBegin(ScaleGestureDetector detector)
    {
        return true;
    }

    public void OnScaleEnd(ScaleGestureDetector detector)
    {
    }

    public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
    {
        this.editor.ScrollBarEnabled = true;
        var endX = this.editor.ScrollX + (int)distanceX;
        var endY = this.editor.ScrollY + (int)distanceY;
        endX = Math.Max(endX, 0);
        endY = Math.Max(endY, 0);
        endX = Math.Min(endX, this.editor.GetMaxScrollX());
        endY = Math.Min(endY, this.editor.GetMaxScrollY());
        this.Scroller.StartScroll(this.editor.ScrollX, this.editor.ScrollY, endX - this.editor.ScrollX, endY - this.editor.ScrollY, 0);
        this.editor.Invalidate();
        return true;
    }

    public void OnShowPress(MotionEvent e)
    {
    }

    public bool OnSingleTapConfirmed(MotionEvent e)
    {
        var lineNumberWidth = 55 + (int)this.editor.Paints.GetLineNumberWidth(this.editor.GetEndVisibleRow() + 1);
        var x = e.GetX() + this.Scroller.CurrX - lineNumberWidth;
        var y = e.GetY() + this.Scroller.CurrY;
        var row = Math.Min((int)y / this.editor.Paints.RowHeight, this.editor.Text.CurrLines.Count - 1);
        var lineText = this.editor.Text.GetLineText(row);
        var column = -1;
        for (int i = 1; i <= lineText.Length; i++)
        {
            var width = this.editor.Paints.MeasureText(lineText, 0, i);
            if (width == x)
            {
                column = i;
            }
            else if (width > x)
            {
                column = i - 1;
                break;
            }
        }

        if (column is -1)
        {
            column = Math.Max(lineText.Length, 0);
        }

        this.editor.UpdateCursor(row, column);
        this.editor.OpenIME();
        return true;
    }

    public bool OnSingleTapUp(MotionEvent e)
    {
        return true;
    }

    private bool scrollBarOnDown = false;

    public bool OnTouchEvent(MotionEvent e)
    {
        var x = (int)e.GetX();
        var y = (int)e.GetY();
        switch (e.Action)
        {
            case MotionEventActions.Down:
            {
                if (this.editor.ScrollBarEnabled)
                {
                    var bounds = this.editor.GetScrollBarBounds();
                    if (bounds.Contains(x + this.Scroller.CurrX, y + this.Scroller.CurrY))
                    {
                        this.scrollBarOnDown = true;
                    }
                }
                return false;
            }
            case MotionEventActions.Move:
            {
                if (this.scrollBarOnDown)
                {
                    var height = (float)this.editor.Height;
                    var maxScrollY = this.editor.GetMaxScrollY();
                    var maxHeight = maxScrollY + height;
                    var toY = Math.Min(Math.Max(y / height * maxHeight, 0), maxScrollY);
                    var dy = toY - this.Scroller.CurrY;
                    this.Scroller.StartScroll(this.Scroller.CurrX, this.Scroller.CurrY, 0, (int)dy, 0);
                    this.editor.Invalidate();
                    return true;
                }
                return false;
            }
            case MotionEventActions.Up:
            case MotionEventActions.Cancel:
            {
                this.scrollBarOnDown = false;
                return false;
            }
        }
        return false;
    }
}
