using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Views;

namespace SensorsAndPeripherals.Navigation
{
    public partial class AppShell : Shell
    {
        private readonly string defaultRoute = nameof(AboutAppPage);
        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            var currentRoute = CurrentItem?.Route;
            if (!string.IsNullOrEmpty(currentRoute))
            {
                Preferences.Set(LocalStorageKeys.DEFAULT_MODULE, currentRoute);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var savedModule = Preferences.Get(LocalStorageKeys.DEFAULT_MODULE, string.Empty);
            var targetItem = Items.FirstOrDefault(x => x.Route == savedModule);
            if (targetItem is not null)
            {
                await GoToAsync($"//{savedModule}");
            }
            else
            {
                await GoToAsync($"//{defaultRoute}");
            }
        }
    }
}