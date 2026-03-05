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
            await DisplayAlertAsync("NfcMessage".GetStringFromResource(), content, "OK");
        };
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.CancelCurrentRequests();
    }

    protected override string InfoText => "";
}