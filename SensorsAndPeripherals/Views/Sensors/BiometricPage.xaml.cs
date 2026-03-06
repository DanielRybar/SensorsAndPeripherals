using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views.Sensors;

public partial class BiometricPage : ApplicationPage
{
    private readonly ViewModels.Sensors.BiometricViewModel viewModel;

    public BiometricPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new ViewModels.Sensors.BiometricViewModel();
        viewModel.ShowBiometricTypesDialogRequested += async types =>
        {
            await DisplayAlertAsync("BiometricAvailableTypes".GetStringFromResource(), types, "OK");
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }

    protected override string InfoToolbarItemsText => string.Empty;
    protected override string InfoText => "";
}