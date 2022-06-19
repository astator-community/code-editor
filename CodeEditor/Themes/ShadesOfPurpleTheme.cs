namespace CodeEditor.Themes;
internal struct ShadesOfPurpleTheme : ITheme
{
    public bool IsDarkMode => true;

    public Color Background => Color.ParseColor("#2d2b55");

    public Color TitleBarBackground => Color.ParseColor("#1a1a36");

    public Color CurrentLine => Color.ParseColor("#801a1a36");

    public Color Keyword => Color.ParseColor("#ff9d00");

    public Color Punctuation => Color.ParseColor("#ffd700");

    public Color Operator => Color.ParseColor("#ff9d00");

    public Color NumericLiteral => Color.ParseColor("#ff628c");

    public Color NamespaceName => Color.ParseColor("#fad000");

    public Color ClassName => Color.ParseColor("#fad000");

    public Color InterfaceName => Color.ParseColor("#fad000");

    public Color StructName => Color.ParseColor("#fad000");

    public Color EnumName => Color.ParseColor("#fad000");

    public Color EnumMemberName => Color.ParseColor("#fad000");

    public Color PropertyName => Color.ParseColor("#9effff");

    public Color FieldName => Color.ParseColor("#fad000");

    public Color DelegateName => Color.ParseColor("#fad000");

    public Color EventName => Color.ParseColor("#fad000");

    public Color ConstantName => Color.ParseColor("#fad000");

    public Color MethodName => Color.ParseColor("#fad000");

    public Color ExtensionMethodName => Color.ParseColor("#fad000");

    public Color ParameterName => Color.ParseColor("#fad000");

    public Color LocalName => Color.ParseColor("#fad000");

    public Color Comment => Color.ParseColor("#b362ff");

    public Color String => Color.ParseColor("#a5ff90");

    public Color Normal => Color.ParseColor("#9effff");
}
