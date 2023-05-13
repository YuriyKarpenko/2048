using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace game2048.av.Android;

[Activity(Label = "game2048.av.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
}
