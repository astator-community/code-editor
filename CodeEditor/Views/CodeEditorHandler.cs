using Microsoft.Maui.Handlers;
using ACodeEditorView = CodeEditor.Views.Droid.CodeEditorView;

namespace CodeEditor.Views;
public class CodeEditorViewHandler : ViewHandler<CodeEditorView, ACodeEditorView>
{
    public static PropertyMapper<CodeEditorView, CodeEditorViewHandler> Mapper = new(ViewMapper)
    {
        [nameof(CodeEditorView.Document)] = MapDocument,
        [nameof(CodeEditorView.Theme)] = MapTheme,
        [nameof(CodeEditorView.FontSize)] = MapFontSize,
    };

    public CodeEditorViewHandler() : base(Mapper)
    {

    }

    public CodeEditorViewHandler(PropertyMapper mapper) : base(mapper)
    {

    }

    protected override ACodeEditorView CreatePlatformView()
    {
        var result = new ACodeEditorView(this.Context, this.VirtualView.Theme, this.VirtualView.FontSize);
        return result;
    }

    private static void MapDocument(CodeEditorViewHandler handler, CodeEditorView view)
    {
        if (view.Document is not null)
        {
            handler.PlatformView.SetDocument(view.Document);
        }
    }

    private static void MapTheme(CodeEditorViewHandler handler, CodeEditorView view)
    {
        handler.PlatformView.SetTheme(view.Theme);
    }

    private static void MapFontSize(CodeEditorViewHandler handler, CodeEditorView view)
    {
        handler.PlatformView.SetTextSize(view.FontSize);
    }
}
