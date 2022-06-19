using Android.Views.InputMethods;
using CodeEditor.Content;
using CodeEditor.Views.Droid;
using Java.Lang;

namespace CodeEditor.Handlers.Droid;
public class InputConnection : BaseInputConnection
{
    private readonly CodeEditorView editor;
    private ContentText Text => this.editor.Text;
    private Cursor Cursor => this.editor.Cursor;

    public InputConnection(CodeEditorView editor) : base(editor, true)
    {
        this.editor = editor;
    }

    public override void CloseConnection()
    {
        base.CloseConnection();
    }

    public override bool BeginBatchEdit()
    {
        return true;
    }

    public override bool CommitText(ICharSequence charSequence, int newCursorPosition)
    {
        var text = charSequence.ToString();
        if (text is "\n")
            this.editor.Text.InsertEnter();
        else
            this.editor.CommitText(text);

        return true;
    }

    public override bool DeleteSurroundingText(int beforeLength, int afterLength)
    {
        if (this.editor.Cursor.Position > 0)
        {
            this.editor.Text.Remove(this.editor.Cursor.Position - 1, 1);
        }
        return true;
    }

    public override bool SendKeyEvent(KeyEvent e)
    {
        if (e.Action != KeyEventActions.Down)
        {
            return false;
        }

        switch (e.KeyCode)
        {
            case Keycode.Del:
            {
                var position = this.Cursor.Position;
                if (this.Cursor.Position > 0)
                {
                    this.Text.Remove(position - 1, 1);
                }
                return true;
            }
            case Keycode.Enter:
            {
                this.editor.Text.InsertEnter();
                return true;
            }
            case Keycode.DpadLeft:
            {
                this.editor.UpdateCursor(this.Cursor.Position - 1);
                return true;
            }
            case Keycode.DpadUp:
            {
                this.editor.UpdateCursor(this.Cursor.CurrRow - 1, this.Cursor.CurrColumn);
                return true;
            }
            case Keycode.DpadRight:
            {
                this.editor.UpdateCursor(this.Cursor.Position + 1);
                return true;
            }
            case Keycode.DpadDown:
            {
                this.editor.UpdateCursor(this.Cursor.CurrRow + 1, this.Cursor.CurrColumn);
                return true;
            }
        }

        return false;
    }
}
