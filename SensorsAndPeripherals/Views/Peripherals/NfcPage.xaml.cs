using SensorsAndPeripherals.ViewModels.Peripherals;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Peripherals;

public partial class NfcPage : ApplicationPage
{
    private readonly NfcViewModel viewModel;

    public NfcPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new NfcViewModel();
    }

    protected override string InfoText => "";
}