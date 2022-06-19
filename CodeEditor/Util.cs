namespace CodeEditor;
internal static class Util
{
    private static readonly double dp = Android.App.Application.Context.Resources.DisplayMetrics.Density;

    public static int Dp2Px(int value)
    {
        return (int)(dp * value);
    }
}
