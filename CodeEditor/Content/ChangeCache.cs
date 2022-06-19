namespace CodeEditor.Content;
public class ChangeCache
{
    public TextChange UndoChange { get; init; }
    public int UndoEditPosition { get; init; }
    public TextChange RedoChange { get; init; }
    public int RedoEditPosition { get; init; }

    public ChangeCache(TextChange undoChange, int undoEditPosition, TextChange redoChange, int redoEditPosition)
    {
        this.UndoChange = undoChange;
        this.RedoChange = redoChange;
        this.UndoEditPosition = undoEditPosition;
        this.RedoEditPosition = redoEditPosition;
    }
}
