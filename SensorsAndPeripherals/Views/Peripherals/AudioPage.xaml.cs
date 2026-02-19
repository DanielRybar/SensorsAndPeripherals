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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel.IsRecording)
        {
            viewModel.StopRecording();
        }
    }

    protected override string InfoText => "";
}