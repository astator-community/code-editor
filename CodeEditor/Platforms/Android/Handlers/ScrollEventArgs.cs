namespace CodeEditor.Handlers.Droid;
public class ScrollEventArgs
{
    public int ScrollX { get; init; }
    public int ScrollY { get; init; }
    public int StartRow { get; init; }
    public int EndRow { get; init; }

    public ScrollEventArgs(int x, int y, int startRow, int endRow)
    {
        this.ScrollX = x;
        this.ScrollY = y;
        this.StartRow = startRow;
        this.EndRow = endRow;
    }
}
