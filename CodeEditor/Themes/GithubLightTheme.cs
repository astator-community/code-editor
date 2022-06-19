
namespace CodeEditor.Themes;
public struct GithubLightTheme : ITheme
{
    public bool IsDarkMode => false;
    public Color Background => Color.ParseColor("#ffffff");
    public Color TitleBarBackground => Color.ParseColor("#e2e4e6");
    public Color CurrentLine => Color.ParseColor("#80e2e4e6");
    public Color Keyword => Color.ParseColor("#d73a49");
    public Color Punctuation => Color.ParseColor("#24292e");
    public Color Operator => Color.ParseColor("#d73a49");
    public Color NumericLiteral => Color.ParseColor("#005cc5");
    public Color NamespaceName => Color.ParseColor("#6f42c1");
    public Color ClassName => Color.ParseColor("#6f42c1");
    public Color InterfaceName => Color.ParseColor("#6f42c1");
    public Color StructName => Color.ParseColor("#6f42c1");
    public Color EnumName => Color.ParseColor("#6f42c1");
    public Color EnumMemberName => Color.ParseColor("#24292e");
    public Color PropertyName => Color.ParseColor("#24292e");
    public Color FieldName => Color.ParseColor("#6f42c1");
    public Color ConstantName => Color.ParseColor("#005cc5");
    public Color DelegateName => Color.ParseColor("#6f42c1");
    public Color EventName => Color.ParseColor("#24292e");
    public Color MethodName => Color.ParseColor("#6f42c1");
    public Color ExtensionMethodName => Color.ParseColor("#6f42c1");
    public Color ParameterName => Color.ParseColor("#e36209");
    public Color LocalName => Color.ParseColor("#6f42c1");
    public Color Comment => Color.ParseColor("#6a737d");
    public Color String => Color.ParseColor("#032f62");
    public Color Normal => Color.ParseColor("#24292e");
}
