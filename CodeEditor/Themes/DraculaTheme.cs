
namespace CodeEditor.Themes;
public struct DraculaTheme : ITheme
{
    public bool IsDarkMode => true;
    public Color Background => Color.ParseColor("#282a36");
    public Color TitleBarBackground => Color.ParseColor("#17181f");
    public Color CurrentLine => Color.ParseColor("#8017181f");
    public Color Keyword => Color.ParseColor("#ff79c6");
    public Color Punctuation => Color.ParseColor("#f8f8f2");
    public Color Operator => Color.ParseColor("#ff79c6");
    public Color NumericLiteral => Color.ParseColor("#bd93f9");
    public Color NamespaceName => Color.ParseColor("#8be9fd");
    public Color ClassName => Color.ParseColor("#8be9fd");
    public Color InterfaceName => Color.ParseColor("#8be9fd");
    public Color StructName => Color.ParseColor("#8be9fd");
    public Color EnumName => Color.ParseColor("#8be9fd");
    public Color EnumMemberName => Color.ParseColor("#f8f8f2");
    public Color PropertyName => Color.ParseColor("#f8f8f2");
    public Color FieldName => Color.ParseColor("#f8f8f2");
    public Color ConstantName => Color.ParseColor("#bd93f9");
    public Color DelegateName => Color.ParseColor("#8be9fd");
    public Color EventName => Color.ParseColor("#f8f8f2");
    public Color MethodName => Color.ParseColor("#50fa7b");
    public Color ExtensionMethodName => Color.ParseColor("#50fa7b");
    public Color ParameterName => Color.ParseColor("#ffb86c");
    public Color LocalName => Color.ParseColor("#f8f8f2");
    public Color Comment => Color.ParseColor("#6272a4");
    public Color String => Color.ParseColor("#f1fa8c");
    public Color Normal => Color.ParseColor("#f8f8f2");
}
