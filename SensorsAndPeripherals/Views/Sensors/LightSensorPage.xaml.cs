using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class LightSensorPage : ApplicationPage
{
    private readonly LightSensorViewModel viewModel;

    public LightSensorPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new LightSensorViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsMonitoring)
        {
            viewModel.StopSensor();
        }
    }

    protected override string InfoText => "";
}