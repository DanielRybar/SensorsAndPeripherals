using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class GyroscopePage : ApplicationPage
{
    private readonly GyroscopeViewModel viewModel;

    public GyroscopePage()
    {
        InitializeComponent();
        BindingContext = viewModel = new GyroscopeViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsMonitoring)
        {
            viewModel.StopSensor();
        }
    }

    protected override string InfoText => "GyroscopeInfoPopup".SafeGetResource<string>();
}