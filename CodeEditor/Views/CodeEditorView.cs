using CodeEditor.Themes;
using ACodeEditorView = CodeEditor.Views.Droid.CodeEditorView;

namespace CodeEditor.Views;
public class CodeEditorView : Microsoft.Maui.Controls.View
{
    public static readonly BindableProperty DocumentProperty = BindableProperty.Create(nameof(Document), typeof(Document), typeof(CodeEditorView));
    public Document Document
    {
        get => this.GetValue(DocumentProperty) as Document;
        set => this.SetValue(DocumentProperty, value);
    }

    public static readonly BindableProperty ThemeProperty = BindableProperty.Create(nameof(Theme), typeof(ITheme), typeof(CodeEditorView), new GithubLightTheme());
    public ITheme Theme
    {
        get => (ITheme)this.GetValue(ThemeProperty);
        set => this.SetValue(ThemeProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(int), typeof(CodeEditorView), 20);
    public int FontSize
    {
        get => (int)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }

    public void Undo()
    {
        (this.Handler.PlatformView as ACodeEditorView).Undo();
    }

    public void Redo()
    {
        (this.Handler.PlatformView as ACodeEditorView).Redo();
    }
    public void CommitSymbol(string value)
    {
        (this.Handler.PlatformView as ACodeEditorView).CommitSymbol(value);
    }

    public void FormatCode()
    {
        (this.Handler.PlatformView as ACodeEditorView).FormatCode();
    }


    public CodeEditorView() : base()
    {

    }
}
