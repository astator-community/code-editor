using CodeEditor.Views;
using CommunityToolkit.Maui;

namespace SampleApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("CascadiaMono.ttf", "CascadiaMono");
            })
            .ConfigureMauiHandlers(handler =>
            {
                handler.AddHandler(typeof(CodeEditorView), typeof(CodeEditorViewHandler));
            });

        return builder.Build();
    }
}
