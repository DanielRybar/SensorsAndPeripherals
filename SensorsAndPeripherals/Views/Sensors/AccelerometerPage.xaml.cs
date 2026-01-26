using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class AccelerometerPage : ApplicationPage
{
	private readonly AccelerometerViewModel viewModel;
	public AccelerometerPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new AccelerometerViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.StopSensor();
    }

    protected override string InfoText => "";
}