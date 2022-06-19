
namespace CodeEditor.Themes;
public interface ITheme
{
    public bool IsDarkMode { get; }
    /// <summary>
    /// 编辑器背景
    /// </summary>
    public Color Background { get; }
    /// <summary>
    /// 标题栏背景
    /// </summary>
    public Color TitleBarBackground { get; }
    /// <summary>
    /// 当前焦点行突出
    /// </summary>
    public Color CurrentLine { get; }

    /// <summary>
    /// 关键字
    /// </summary>
    public Color Keyword { get; }
    /// <summary>
    /// 标点符号
    /// </summary>
    public Color Punctuation { get; }
    /// <summary>
    /// 操作符
    /// </summary>
    public Color Operator { get; }
    /// <summary>
    /// 数值
    /// </summary>
    public Color NumericLiteral { get; }
    /// <summary>
    /// 命名空间
    /// </summary>
    public Color NamespaceName { get; }
    /// <summary>
    /// 类
    /// </summary>
    public Color ClassName { get; }
    /// <summary>
    /// 接口
    /// </summary>
    public Color InterfaceName { get; }
    /// <summary>
    /// 结构体
    /// </summary>
    public Color StructName { get; }
    /// <summary>
    /// 枚举
    /// </summary>
    public Color EnumName { get; }
    /// <summary>
    /// 枚举成员
    /// </summary>
    public Color EnumMemberName { get; }
    /// <summary>
    /// 属性
    /// </summary>
    public Color PropertyName { get; }
    /// <summary>
    /// 字段
    /// </summary>
    public Color FieldName { get; }
    /// <summary>
    /// 委托
    /// </summary>
    public Color DelegateName { get; }
    /// <summary>
    /// 事件
    /// </summary>
    public Color EventName { get; }
    /// <summary>
    /// 常量
    /// </summary>
    public Color ConstantName { get; }
    /// <summary>
    /// 方法/函数
    /// </summary>
    public Color MethodName { get; }
    /// <summary>
    /// 扩展方法/函数
    /// </summary>
    public Color ExtensionMethodName { get; }
    /// <summary>
    /// 参数
    /// </summary>
    public Color ParameterName { get; }
    /// <summary>
    /// 本地变量
    /// </summary>
    public Color LocalName { get; }
    /// <summary>
    /// 注释
    /// </summary>
    /// <summary>
    /// 字符串
    /// </summary>
    public Color Comment { get; }
    /// <summary>
    /// 字符串
    /// </summary>
    public Color String { get; }
    public Color Normal { get; }

}
