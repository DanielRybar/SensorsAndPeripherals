using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Peripherals;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Peripherals;

public partial class AudioPage : ApplicationPage
{
    private readonly AudioViewModel viewModel;

    public AudioPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new AudioViewModel();
    }

    protected override void OnSafeDisappearing()
    {
        if (viewModel.IsRecording)
        {
            viewModel.StopRecording();
        }
        viewModel.StopPlayback();
    }

    protected override string InfoText => "AudioInfoPopup".SafeGetResource<string>();
}