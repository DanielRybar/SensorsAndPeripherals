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
            await DisplayAlertAsync("Dostupné typy ovìøení", types, "OK");
        };
    }
    
    protected override string InfoText => "";
}