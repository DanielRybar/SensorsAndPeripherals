using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Peripherals;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Peripherals;

public partial class VibrationPage : ApplicationPage
{
    private readonly VibrationViewModel viewModel;

    public VibrationPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new VibrationViewModel();
    }

    protected override void OnSafeDisappearing()
    {
        viewModel.CancelVibration();
    }

    protected override string InfoText => "VibrationInfoPopup".SafeGetResource<string>();
}