using SensorsAndPeripherals.Views.Abstract;

namespace SensorsAndPeripherals.Views;

public partial class AboutAppPage : ApplicationPage
{
	public AboutAppPage() : base(showInfoToolbarItem: false)
	{
		InitializeComponent();
	}

	protected override string InfoText => "";
}