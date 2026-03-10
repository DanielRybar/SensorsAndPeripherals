using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Peripherals;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Peripherals;

public partial class ConnectivityPage : ApplicationPage
{
    private readonly ConnectivityViewModel viewModel;

    public ConnectivityPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new ConnectivityViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.GetConnectionStatusCommand.Execute(null);
    }

    protected override string InfoText => "ConnectivityInfoPopup".GetStringFromResource();
}