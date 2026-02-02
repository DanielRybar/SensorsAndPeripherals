using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Platform;

namespace SensorsAndPeripherals
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity
    {
        private EventHandler<AppThemeChangedEventArgs>? themeChangedHandler;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetDecorViewBackgroundColor();
            themeChangedHandler = (sender, args) => SetDecorViewBackgroundColor();
            App.Current?.RequestedThemeChanged += themeChangedHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            App.Current?.RequestedThemeChanged -= themeChangedHandler;
        }

        private void SetDecorViewBackgroundColor()
        {
            var resources = App.Current?.Resources;
            if (resources is not null)
            {
                if (App.Current?.RequestedTheme == AppTheme.Dark)
                {
                    Window?.DecorView?.SetBackgroundColor((resources["OffBlack"] as Color)!.ToPlatform());
                }
                else
                {
                    Window?.DecorView?.SetBackgroundColor((resources["White"] as Color)!.ToPlatform());
                }
            }
        }
    }
}