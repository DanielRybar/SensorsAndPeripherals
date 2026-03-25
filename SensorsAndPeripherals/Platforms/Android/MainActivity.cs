using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;
using Plugin.NFC;

namespace SensorsAndPeripherals
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter([
        Android.Nfc.NfcAdapter.ActionNdefDiscovered,
        Android.Nfc.NfcAdapter.ActionTagDiscovered,
        Android.Nfc.NfcAdapter.ActionTechDiscovered],
        Categories = new[] { Intent.CategoryDefault })]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossNFC.Init(this);
            if (Window is not null)
            {
                var windowInsetsController = WindowCompat.GetInsetsController(Window, Window.DecorView);
                windowInsetsController?.AppearanceLightNavigationBars = false;
                windowInsetsController?.AppearanceLightStatusBars = false;
            }
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            CrossNFC.OnNewIntent(intent);
        }
    }
}