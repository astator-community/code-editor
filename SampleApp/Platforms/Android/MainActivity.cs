using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SampleApp;
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Instance;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        Instance = this;
        base.OnCreate(savedInstanceState);
    }
}
