using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Peripherals;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Peripherals;

public partial class BluetoothPage : ApplicationPage
{
    private readonly BluetoothViewModel viewModel;

    public BluetoothPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new BluetoothViewModel();
        viewModel.ShowAdapterInfoDialogRequested += async name =>
        {
            await DisplayAlertAsync("BluetoothAdapterName".GetStringFromResource(), name, "OK");
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.Initialize();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.CleanUp();
    }

    protected override string InfoToolbarItemsText => string.Empty;
    protected override string InfoText => "";
}