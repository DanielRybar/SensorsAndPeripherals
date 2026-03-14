using SensorsAndPeripherals.Helpers;
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
        viewModel.ShowReadingResultRequested += async content =>
        {
            await DisplayAlertAsync("NfcMessage".SafeGetResource<string>(), content, "OK");
        };
    }

    protected override void OnSafeDisappearing()
    {
        viewModel.CancelCurrentRequests();
    }

    protected override string InfoText => "NfcInfoPopup".SafeGetResource<string>();
}