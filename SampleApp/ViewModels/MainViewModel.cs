using Android.Views;
using CodeEditor.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace SampleApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private Color backgroundColor = Colors.Black;

    [ObservableProperty]
    private Color textColor = Colors.White;

    [ObservableProperty]
    private string undoImageSource;

    [ObservableProperty]
    private string redoImageSource;

    [ObservableProperty]
    private string moreImageSource;

    [ObservableProperty]
    private Document document;

    [ObservableProperty]
    private ITheme theme;


    public MainViewModel()
    {
        this.Theme = ThemeExtensions.GetTheme("Night Owl");
        this.CreateDocument();
    }


    [RelayCommand]
    public void CommitSymbol(object parameter)
    {
        WeakReferenceMessenger.Default.Send(new CommandMessage(new Message
        {
            Type = MessageType.SendSymbol,
            Value = parameter.ToString()
        }));
    }

    [RelayCommand]
    public void SendUndo()
    {
        WeakReferenceMessenger.Default.Send(new CommandMessage(new Message
        {
            Type = MessageType.SendUndo
        }));
    }

    [RelayCommand]
    public void SendRedo()
    {
        WeakReferenceMessenger.Default.Send(new CommandMessage(new Message
        {
            Type = MessageType.SendRedo
        }));
    }

    [RelayCommand]
    public void OpenOptions()
    {
        WeakReferenceMessenger.Default.Send(new CommandMessage(new Message
        {
            Type = MessageType.OpenOptions,
            Value = (this.BackgroundColor, this.TextColor),
        }));
    }


    private void CreateDocument()
    {
        var workspace = new AdhocWorkspace();
        var references = new List<MetadataReference>();

        var files = Android.App.Application.Context.Assets.List("Resources/References");
        foreach (var file in files)
        {
            if (file.EndsWith(".dll"))
            {
                DocumentationProvider xmlProvider = null;
                var name = Path.GetFileNameWithoutExtension(file);
                var xmlPath = $"{name}.xml";
                if (files.Contains(xmlPath))
                {
                    using Stream stream = Android.App.Application.Context.Assets.Open(Path.Combine("Resources/References", xmlPath));
                    using MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    xmlProvider = XmlDocumentationProvider.CreateFromBytes(ms.ToArray());
                }

                {
                    using Stream stream = Android.App.Application.Context.Assets.Open(Path.Combine("Resources/References", file));
                    using MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    references.Add(MetadataReference.CreateFromStream(ms, documentation: xmlProvider));
                }

            }
        }

        var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(),
            VersionStamp.Create(),
            "TestProject",
            "TestProject",
            LanguageNames.CSharp).
            WithMetadataReferences(references);
        workspace.AddProject(projectInfo);

        {
            using Stream stream = Android.App.Application.Context.Assets.Open(Path.Combine("Resources/Files", "default-doc.txt"));
            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            var text = Encoding.UTF8.GetString(ms.ToArray()).Replace("\r\n", "\n").Replace("\t", "    ");

            this.Document = workspace.AddDocument(projectInfo.Id, "Test.cs", SourceText.From(text));
        }
    }

    partial void OnThemeChanged(ITheme value)
    {
        this.BackgroundColor = Color.FromInt(value.TitleBarBackground);

        var window = MainActivity.Instance.Window;
        window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        window.SetStatusBarColor(value.TitleBarBackground);

        if (value.IsDarkMode)
        {
            this.TextColor = Colors.White;
            this.UndoImageSource = "undo_white.png";
            this.RedoImageSource = "redo_white.png";
            this.MoreImageSource = "more_white.png";

            window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
        }
        else
        {
            this.TextColor = Colors.Black;
            this.UndoImageSource = "undo_black.png";
            this.RedoImageSource = "redo_black.png";
            this.MoreImageSource = "more_black.png";

            window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
        }
    }
}
