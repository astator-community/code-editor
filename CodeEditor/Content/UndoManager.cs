namespace CodeEditor.Content;
public class UndoManager
{
    private readonly List<ChangeCache> changeCaches = new();
    private int position = -1;

    public void Add(TextChange undoChange, int undoEditPosition, TextChange redoChange, int redoEditPosition)
    {
        if (this.position < this.changeCaches.Count - 1)
        {
            this.changeCaches.RemoveRange(this.position + 1, this.changeCaches.Count - this.position - 1);
        }
        this.changeCaches.Add(new ChangeCache(undoChange, undoEditPosition, redoChange, redoEditPosition));
        this.position = this.changeCaches.Count;
    }

    public (TextChange, int) GetUndoCache()
    {
        if (this.changeCaches.Count > 0 && this.position >= 0)
        {
            this.position = Math.Min(this.position, this.changeCaches.Count - 1);
            var cache = this.changeCaches.ElementAt(this.position);
            this.position -= 1;
            return (cache.UndoChange, cache.UndoEditPosition);
        }
        return (new TextChange(new TextSpan(0, 0), string.Empty), -1);
    }

    public (TextChange, int) GetRedoCache()
    {
        if (this.changeCaches.Count > 0 && this.position < this.changeCaches.Count - 1)
        {
            var cache = this.changeCaches.ElementAt(this.position + 1);
            this.position += 1;
            return (cache.RedoChange, cache.RedoEditPosition);
        }
        return (new TextChange(new TextSpan(0, 0), string.Empty), -1);
    }
}
