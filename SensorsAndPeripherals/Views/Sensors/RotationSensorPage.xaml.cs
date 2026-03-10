using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class RotationSensorPage : ApplicationPage
{
    private readonly RotationSensorViewModel viewModel;

    public RotationSensorPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new RotationSensorViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsMonitoring)
        {
            viewModel.StopSensor();
        }
    }

    protected override string InfoText => "RotationSensorInfoPopup".SafeGetResource<string>();
}