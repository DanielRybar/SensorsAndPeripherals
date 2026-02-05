using SensorsAndPeripherals.ViewModels.Sensors;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class GpsPage : ApplicationPage
{
    private readonly GpsViewModel viewModel;

    public GpsPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new GpsViewModel();
        viewModel.ShowAddressDialogRequested += async address =>
        {
            await DisplayAlertAsync("Adresa", address, "OK");
        };
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.CancelCurrentLocationRequest();
    }

    protected override string InfoText => "";
}