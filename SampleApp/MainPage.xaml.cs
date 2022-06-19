using CodeEditor.Themes;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        WeakReferenceMessenger.Default.Register<CommandMessage>(this, (_, m) =>
        {
            var message = m.Value;

            switch (message.Type)
            {
                case MessageType.SendUndo: this.CodeEditor.Undo(); break;
                case MessageType.SendRedo: this.CodeEditor.Redo(); break;
                case MessageType.SendSymbol:
                {
                    if (message.Value is not null)
                    {
                        this.CodeEditor.CommitSymbol(message.Value as string);
                    }
                    break;
                }
                case MessageType.OpenOptions:
                {
                    var (backgroundColor, textColor) = ((Color, Color))message.Value;
                    this.ShowOptionsPopup(backgroundColor, textColor);
                    break;
                }
            }
        });
    }

    private async void ShowOptionsPopup(Color backgroundColor, Color textColor)
    {
        var changeThemeBtn = new Button
        {
            Text = "切换主题",
            TextColor = textColor,
            BackgroundColor = backgroundColor,
            FontSize = 16,
            Padding = new Thickness(25, 15),
        };

        var changeDocumentBtn = new Button
        {
            Text = "切换文档",
            TextColor = textColor,
            BackgroundColor = backgroundColor,
            FontSize = 16,
            Padding = new Thickness(25, 15),
        };

        var changeFontSizeBtn = new Button
        {
            Text = "字体大小",
            TextColor = textColor,
            BackgroundColor = backgroundColor,
            FontSize = 16,
            Padding = new Thickness(25, 15),
        };

        var formatCodeBtn = new Button
        {
            Text = "代码格式化",
            TextColor = textColor,
            BackgroundColor = backgroundColor,
            FontSize = 16,
            Padding = new Thickness(25, 15),
        };

        var popup = new Popup
        {
            Content = new VerticalStackLayout
            {
                Children =
                {
                    changeThemeBtn,
                    changeDocumentBtn,
                    changeFontSizeBtn,
                    formatCodeBtn
                }
            },
            Anchor = this.Options,
            Color = backgroundColor,
        };

        changeThemeBtn.Clicked += (s, e) =>
        {
            this.ShowThemeListPopup(backgroundColor, textColor);
            popup.Close();
        };

        changeDocumentBtn.Clicked += (s, e) =>
        {
            this.ShowDocumentListPopup(backgroundColor, textColor);
            popup.Close();
        };

        changeFontSizeBtn.Clicked += (s, e) =>
        {
            this.ShowFontSizeListPopup(backgroundColor, textColor);
            popup.Close();
        };

        formatCodeBtn.Clicked += (s, e) =>
        {
            this.CodeEditor.FormatCode();
            popup.Close();
        };

        await this.ShowPopupAsync(popup);
    }

    private async void ShowFontSizeListPopup(Color backgroundColor, Color textColor)
    {
        var root = new VerticalStackLayout();
        var popup = new Popup
        {
            Content = root,
            Anchor = this.Options,
            Color = backgroundColor,
        };

        var sizes = new string[] { "12", "14", "16", "18", "20", "22", "24", "26", "28", "30" };
        foreach (var size in sizes)
        {
            var btn = new Button
            {
                Text = size,
                FontSize = 18,
                Padding = new Thickness(35, 15),
                TextColor = textColor,
                BackgroundColor = backgroundColor,
            };
            btn.Clicked += (s, e) =>
            {
                this.CodeEditor.FontSize = Convert.ToInt32((s as Button).Text);
                popup.Close();
            };
            root.Add(btn);
        }

        await this.ShowPopupAsync(popup);
    }

    private async void ShowDocumentListPopup(Color backgroundColor, Color textColor)
    {
        var root = new VerticalStackLayout();
        var popup = new Popup
        {
            Content = root,
            Anchor = this.Options,
            Color = backgroundColor,
        };

        var names = new string[] { "默认文本", "大文本" };
        foreach (var name in names)
        {
            var btn = new Button
            {
                Text = name,
                FontSize = 16,
                Padding = new Thickness(25, 15),
                TextColor = textColor,
                BackgroundColor = backgroundColor,
            };
            btn.Clicked += (s, e) =>
            {
                using Stream stream = Android.App.Application.Context.Assets.Open(Path.Combine("Resources/Files", (s as Button).Text == "默认文本" ? "default-doc.txt" : "big-doc.txt"));
                using MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;
                var text = Encoding.UTF8.GetString(ms.ToArray());
                this.CodeEditor.Document = this.CodeEditor.Document.WithText(SourceText.From(text));
                popup.Close();
            };
            root.Add(btn);
        }

        await this.ShowPopupAsync(popup);
    }

    private async void ShowThemeListPopup(Color backgroundColor, Color textColor)
    {
        var root = new VerticalStackLayout();
        var popup = new Popup
        {
            Content = root,
            Anchor = this.Options,
            Color = backgroundColor,
        };

        var names = ThemeExtensions.GetThemeNames();
        foreach (var name in names)
        {
            var btn = new Button
            {
                Text = name,
                FontSize = 16,
                Padding = new Thickness(25, 15),
                TextColor = textColor,
                BackgroundColor = backgroundColor,
            };
            btn.Clicked += (s, e) =>
            {
                this.CodeEditor.Theme = ThemeExtensions.GetTheme((s as Button).Text);
                popup.Close();
            };
            root.Add(btn);
        }

        await this.ShowPopupAsync(popup);
    }
}



