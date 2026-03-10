using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class BarometerPage : ApplicationPage
{
    private readonly BarometerViewModel viewModel;

    public BarometerPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new BarometerViewModel();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsMonitoring)
        {
            viewModel.StopSensor();
        }
    }

    protected override string InfoText => "BarometerInfoPopup".GetStringFromResource();
}