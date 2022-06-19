namespace CodeEditor.Handlers.Droid;
internal class Cursor
{
    public int X { get; set; } = 113;
    public int Y { get; set; } = 0;
    public int Position { get; set; } = 0;
    public int CurrRow { get; set; } = 0;
    public int CurrColumn { get; set; } = 0;
    public bool Showing { get; set; } = true;
    public DateTime UpdateTime { get; set; } = DateTime.Now;

    public bool CheckDraw(int startRow, int endRow)
    {
        return this.Showing && this.CurrRow >= startRow && this.CurrRow <= endRow;
    }

    public void Update(int position, int x, int y, int row, int column)
    {
        this.Position = position;
        this.X = x;
        this.Y = y;
        this.CurrRow = row;
        this.CurrColumn = column;
        this.UpdateTime = DateTime.Now;
        this.Showing = true;
    }
}
