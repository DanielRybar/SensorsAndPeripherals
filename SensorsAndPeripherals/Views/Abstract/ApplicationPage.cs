using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;

namespace SensorsAndPeripherals.Views.Abstract
{
    public abstract partial class ApplicationPage : ContentPage
    {
        protected abstract string InfoText { get; }

        public ApplicationPage(bool showInfoToolbarItem = true)
        {
            if (showInfoToolbarItem)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    IconImageSource = App.Current!.Resources["ToolbarHelpIcon"] as FontImageSource,
                    Command = new Command(async () =>
                    {
                        await DisplayAlertAsync("Informace", InfoText, "OK");
                    })
                });
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(App.Current!.Resources["BarColor"] as Color ?? Colors.Black);
        }
    }
}