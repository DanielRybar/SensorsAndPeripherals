using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Peripherals;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Peripherals;

public partial class CameraPage : ApplicationPage
{
    private readonly CameraViewModel viewModel;

    public CameraPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new CameraViewModel();
    }

    protected override string InfoText => "CameraInfoPopup".GetStringFromResource();
}