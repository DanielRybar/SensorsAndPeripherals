using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;

namespace SensorsAndPeripherals.Views.Abstract
{
    public abstract partial class ApplicationPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(App.Current!.Resources["BarColor"] as Color ?? Colors.Black);
        }
    }
}