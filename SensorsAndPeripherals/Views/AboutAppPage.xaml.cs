using SensorsAndPeripherals.ViewModels;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views;

public partial class AboutAppPage : ApplicationPage
{
	private readonly AboutAppViewModel viewModel;

	public AboutAppPage() : base(showInfoToolbarItem: false)
	{
		InitializeComponent();
		BindingContext = viewModel = new AboutAppViewModel();
    }

    private async void DevImage_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Image img)
        {
            switch (img.AutomationId)
            {
                case "csharp":
                    await Launcher.OpenAsync("https://learn.microsoft.com/en-us/dotnet/csharp/");
                    break;
                case "net":
                    await Launcher.OpenAsync("https://dotnet.microsoft.com/en-us/");
                    break;
                case "maui":
                    await Launcher.OpenAsync("https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui?view=net-maui-10.0");
                    break;
            }
        }
    }

    protected override string InfoText => "";
}