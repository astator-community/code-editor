
namespace CodeEditor.Themes;
public struct OneDarkProTheme : ITheme
{
    public bool IsDarkMode => true;
    public Color Background => Color.ParseColor("#282c34");
    public Color TitleBarBackground => Color.ParseColor("#21252b");
    public Color CurrentLine => Color.ParseColor("#8021252b");
    public Color Keyword => Color.ParseColor("#c678dd");
    public Color Punctuation => Color.ParseColor("#abb2bf");
    public Color Operator => Color.ParseColor("#abb2bf");
    public Color NumericLiteral => Color.ParseColor("#d19a66");
    public Color NamespaceName => Color.ParseColor("#E5C07B");
    public Color ClassName => Color.ParseColor("#E5C07B");
    public Color InterfaceName => Color.ParseColor("#e5c07b");
    public Color StructName => Color.ParseColor("#e5c07b");
    public Color EnumName => Color.ParseColor("#e5c07b");
    public Color EnumMemberName => Color.ParseColor("#56b6c2");
    public Color PropertyName => Color.ParseColor("#e06c75");
    public Color FieldName => Color.ParseColor("#e06c75");
    public Color ConstantName => Color.ParseColor("#e06c75");
    public Color DelegateName => Color.ParseColor("#e5c07b");
    public Color EventName => Color.ParseColor("#e06c75");
    public Color MethodName => Color.ParseColor("#61afef");
    public Color ExtensionMethodName => Color.ParseColor("#61afef");
    public Color ParameterName => Color.ParseColor("#e06c75");
    public Color LocalName => Color.ParseColor("#e06c75");
    public Color Comment => Color.ParseColor("#7f848e");
    public Color String => Color.ParseColor("#98c379");
    public Color Normal => Color.ParseColor("#e06c75");
}
