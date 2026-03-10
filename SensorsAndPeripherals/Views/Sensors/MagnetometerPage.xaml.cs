using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class MagnetometerPage : ApplicationPage
{
    private readonly MagnetometerViewModel viewModel;

    public MagnetometerPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new MagnetometerViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsMonitoring)
        {
            viewModel.StopSensor();
        }
    }

    protected override string InfoText => "MagnetometerInfoPopup".SafeGetResource<string>();
}