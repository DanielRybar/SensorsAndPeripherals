using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class ProximitySensorPage : ApplicationPage
{
    private readonly ProximitySensorViewModel viewModel;

    public ProximitySensorPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new ProximitySensorViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsMonitoring)
        {
            viewModel.StopSensor();
        }
    }

    protected override string InfoText => "ProximitySensorInfoPopup".SafeGetResource<string>();
}