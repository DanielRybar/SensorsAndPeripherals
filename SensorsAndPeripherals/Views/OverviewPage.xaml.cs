using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels;
using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views;

public partial class OverviewPage : ApplicationPage
{
    private readonly OverviewViewModel viewModel;

    public OverviewPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new OverviewViewModel();
    }

    protected override string InfoText => "OverviewInfoPopup".SafeGetResource<string>();
}