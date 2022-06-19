namespace CodeEditor.Themes;

public static class ThemeExtensions
{
    private static readonly Dictionary<string, Type> Themes = new();

    static ThemeExtensions()
    {
        Themes.Add("Dracula", typeof(DraculaTheme));
        Themes.Add("One Dark Pro", typeof(OneDarkProTheme));
        Themes.Add("Github Light", typeof(GithubLightTheme));
        Themes.Add("Night Owl", typeof(NightOwlTheme));
        Themes.Add("Shades of Purple", typeof(ShadesOfPurpleTheme));
    }

    public static void AddTheme(string key, Type type)
    {
        if (!Themes.ContainsKey(key))
        {
            Themes.Add(key, type);
        }
    }

    public static List<string> GetThemeNames()
    {
        return Themes.Select(c => c.Key).ToList();
    }

    public static ITheme GetTheme(string key)
    {
        if (Themes.ContainsKey(key))
        {
            return Activator.CreateInstance(Themes[key]) as ITheme;
        }
        return default;
    }
}
