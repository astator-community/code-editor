namespace CodeEditor.Themes;
internal struct NightOwlTheme : ITheme
{
    public bool IsDarkMode => true;
    public Color Background => Color.ParseColor("#011627");
    public Color TitleBarBackground => Color.ParseColor("#010c17");
    public Color CurrentLine => Color.ParseColor("#80010c17");
    public Color Keyword => Color.ParseColor("#c792ea");
    public Color Punctuation => Color.ParseColor("#d6deeb");
    public Color Operator => Color.ParseColor("#c792ea");
    public Color NumericLiteral => Color.ParseColor("#f78c6c");
    public Color NamespaceName => Color.ParseColor("#b2ccd6");
    public Color ClassName => Color.ParseColor("#ffcb8b");
    public Color InterfaceName => Color.ParseColor("#e6ee9c");
    public Color StructName => Color.ParseColor("#8cd1f5");
    public Color EnumName => Color.ParseColor("#c5e478");
    public Color EnumMemberName => Color.ParseColor("#d6deeb");
    public Color PropertyName => Color.ParseColor("#d6deeb");
    public Color FieldName => Color.ParseColor("#d6deeb");
    public Color DelegateName => Color.ParseColor("#d6deeb");
    public Color EventName => Color.ParseColor("#d6deeb");
    public Color ConstantName => Color.ParseColor("#ff2c83");
    public Color MethodName => Color.ParseColor("#82aaff");
    public Color ExtensionMethodName => Color.ParseColor("#82aaff");
    public Color ParameterName => Color.ParseColor("#d6deeb");
    public Color LocalName => Color.ParseColor("#d6deeb");
    public Color Comment => Color.ParseColor("#637777");
    public Color String => Color.ParseColor("#ecc48d");
    public Color Normal => Color.ParseColor("#d6deeb");
}
