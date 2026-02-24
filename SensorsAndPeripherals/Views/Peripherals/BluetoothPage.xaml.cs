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
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.StopDiscoveringAndAdvertising();
    }

    protected override string InfoText => "";
}