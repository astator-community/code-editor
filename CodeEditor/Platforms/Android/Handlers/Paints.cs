using Android.Graphics;
using CodeEditor.Themes;
using Microsoft.CodeAnalysis.Classification;
using Paint = Android.Graphics.Paint;

namespace CodeEditor.Handlers.Droid;
internal class Paints
{
    public ITheme Theme { get; private set; }
    public Paint Keyword { get; private set; }
    public Paint Punctuation { get; private set; }
    public Paint Operator { get; private set; }
    public Paint NumericLiteral { get; private set; }
    public Paint Variable { get; private set; }
    public Paint NamespaceName { get; private set; }
    public Paint ClassName { get; private set; }
    public Paint InterfaceName { get; private set; }
    public Paint StructName { get; private set; }
    public Paint EnumName { get; private set; }
    public Paint EnumMemberName { get; private set; }
    public Paint ConstantName { get; private set; }
    public Paint DelegateName { get; private set; }
    public Paint EventName { get; private set; }
    public Paint PropertyName { get; private set; }
    public Paint FieldName { get; private set; }
    public Paint MethodName { get; private set; }
    public Paint ExtensionMethodName { get; private set; }
    public Paint ParameterName { get; private set; }
    public Paint LocalName { get; private set; }
    public Paint Comment { get; private set; }
    public Paint String { get; private set; }
    public Paint Normal { get; private set; }
    public Paint LineNumber { get; private set; }
    public Paint FocusLine { get; private set; }

    public float ENWidth { get; private set; } = 0;
    public float ZHWidth { get; private set; } = 0;
    public int RowHeight { get; private set; } = 0;

    private int textSize = Util.Dp2Px(16);
    private Paint.FontMetrics fontMetrics;

    private readonly Typeface tf;

    public Paints(ITheme theme, int textSize)
    {
        this.Theme = theme;
        //this.tf = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, "CascadiaMono.ttf");
        this.tf = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, "CascadiaCode.ttf");
        this.textSize = textSize;
        this.InitializePaints();
    }

    public void SetTheme(ITheme theme)
    {
        this.Theme = theme;
        this.InitializePaints();
    }

    private void InitializePaints()
    {
        this.Keyword = this.InitializePaint(this.Theme.Keyword);
        this.Punctuation = this.InitializePaint(this.Theme.Punctuation);
        this.Operator = this.InitializePaint(this.Theme.Operator);
        this.NumericLiteral = this.InitializePaint(this.Theme.NumericLiteral);
        this.NamespaceName = this.InitializePaint(this.Theme.NamespaceName);
        this.ClassName = this.InitializePaint(this.Theme.ClassName);
        this.InterfaceName = this.InitializePaint(this.Theme.InterfaceName);
        this.StructName = this.InitializePaint(this.Theme.StructName);
        this.EnumName = this.InitializePaint(this.Theme.EnumName);
        this.EnumMemberName = this.InitializePaint(this.Theme.EnumMemberName);
        this.PropertyName = this.InitializePaint(this.Theme.PropertyName);
        this.FieldName = this.InitializePaint(this.Theme.FieldName);
        this.DelegateName = this.InitializePaint(this.Theme.DelegateName);
        this.EventName = this.InitializePaint(this.Theme.EventName);
        this.MethodName = this.InitializePaint(this.Theme.MethodName);
        this.ConstantName = this.InitializePaint(this.Theme.ConstantName);
        this.ExtensionMethodName = this.InitializePaint(this.Theme.ExtensionMethodName);
        this.ParameterName = this.InitializePaint(this.Theme.ParameterName);
        this.LocalName = this.InitializePaint(this.Theme.LocalName);
        this.Comment = this.InitializePaint(this.Theme.Comment);
        this.String = this.InitializePaint(this.Theme.String);
        this.FocusLine = this.InitializePaint(this.Theme.CurrentLine);
        this.Normal = this.InitializePaint(this.Theme.Normal);

        this.LineNumber = this.InitializePaint(this.Theme.Comment);
        this.LineNumber.TextSize = this.textSize * 0.8f;
        this.LineNumber.TextAlign = Paint.Align.Right;

        this.fontMetrics = this.Normal.GetFontMetrics();
        this.ENWidth = this.Normal.MeasureText("0");
        this.RowHeight = (int)(this.fontMetrics.Bottom - this.fontMetrics.Top);
    }

    private Paint InitializePaint(Color color)
    {
        var result = new Paint { AntiAlias = true };
        result.SetTypeface(this.tf);
        result.TextSize = this.textSize;
        result.SetARGB(color.A, color.R, color.G, color.B);
        return result;
    }

    public void SetTextSize(int value)
    {
        this.textSize = Util.Dp2Px(value);
        this.Keyword.TextSize = this.textSize;
        this.Punctuation.TextSize = this.textSize;
        this.Operator.TextSize = this.textSize;
        this.NumericLiteral.TextSize = this.textSize;
        this.NamespaceName.TextSize = this.textSize;
        this.ClassName.TextSize = this.textSize;
        this.InterfaceName.TextSize = this.textSize;
        this.StructName.TextSize = this.textSize;
        this.EnumName.TextSize = this.textSize;
        this.EnumMemberName.TextSize = this.textSize;
        this.ConstantName.TextSize = this.textSize;
        this.DelegateName.TextSize = this.textSize;
        this.EventName.TextSize = this.textSize;
        this.PropertyName.TextSize = this.textSize;
        this.FieldName.TextSize = this.textSize;
        this.MethodName.TextSize = this.textSize;
        this.ExtensionMethodName.TextSize = this.textSize;
        this.ParameterName.TextSize = this.textSize;
        this.LocalName.TextSize = this.textSize;
        this.Comment.TextSize = this.textSize;
        this.Normal.TextSize = this.textSize;
        this.String.TextSize = this.textSize;
        this.LineNumber.TextSize = this.textSize * 0.8f;

        this.fontMetrics = this.Normal.GetFontMetrics();
        this.ENWidth = this.Normal.MeasureText("a");
        this.ZHWidth = this.Normal.MeasureText("一");
        this.RowHeight = (int)(this.fontMetrics.Bottom - this.fontMetrics.Top);
    }

    public float MeasureText(string text)
    {
        return this.Normal.MeasureText(text);
    }

    public float MeasureText(string text, int start, int end)
    {
        return this.Normal.MeasureText(text, start, end);
    }

    public Paint GetPaint(string type)
    {
        return type switch
        {
            ClassificationTypeNames.Keyword
            or ClassificationTypeNames.PreprocessorKeyword
            or ClassificationTypeNames.ControlKeyword
            => this.Keyword,

            ClassificationTypeNames.Punctuation => this.Punctuation,
            ClassificationTypeNames.Operator => this.Operator,
            ClassificationTypeNames.NumericLiteral => this.NumericLiteral,
            ClassificationTypeNames.NamespaceName => this.NamespaceName,

            ClassificationTypeNames.ClassName
            or ClassificationTypeNames.RecordClassName
            => this.ClassName,

            ClassificationTypeNames.InterfaceName => this.InterfaceName,
            ClassificationTypeNames.StructName => this.StructName,
            ClassificationTypeNames.EnumName => this.EnumName,
            ClassificationTypeNames.EnumMemberName => this.EnumMemberName,
            ClassificationTypeNames.PropertyName => this.PropertyName,
            ClassificationTypeNames.FieldName => this.FieldName,
            ClassificationTypeNames.ConstantName => this.ConstantName,
            ClassificationTypeNames.DelegateName => this.DelegateName,
            ClassificationTypeNames.EventName => this.EventName,
            ClassificationTypeNames.MethodName => this.MethodName,
            ClassificationTypeNames.ExtensionMethodName => this.ExtensionMethodName,
            ClassificationTypeNames.ParameterName => this.ParameterName,
            ClassificationTypeNames.LocalName => this.LocalName,

            ClassificationTypeNames.Comment
            or ClassificationTypeNames.XmlDocCommentDelimiter
            or ClassificationTypeNames.XmlDocCommentName
            or ClassificationTypeNames.XmlDocCommentText
            => this.Comment,

            ClassificationTypeNames.XmlDocCommentAttributeName => this.ParameterName,

            ClassificationTypeNames.StringLiteral
            or ClassificationTypeNames.VerbatimStringLiteral
            or ClassificationTypeNames.PreprocessorText
            or ClassificationTypeNames.RegexText
            => this.String,


            ClassificationTypeNames.StringEscapeCharacter => this.MethodName,

            _ => this.Normal
        };

    }

    public float GetLineNumberWidth(int maxLine)
    {
        return this.ENWidth * maxLine switch
        {
            >= 100000 => 6,
            >= 10000 => 5,
            >= 1000 => 4,
            >= 100 => 3,
            >= 10 => 2,
            _ => 1
        };
    }

    public int GetRowBaseline(int row)
    {
        return (int)((this.RowHeight * (row + 1)) - this.fontMetrics.Bottom);
    }
}
