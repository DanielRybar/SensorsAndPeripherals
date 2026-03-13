using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Views;

namespace SensorsAndPeripherals.Navigation
{
    public partial class AppShell : Shell
    {
        private readonly string defaultRoute = $"//{nameof(AboutAppPage)}";

        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            var currentLocation = args?.Current?.Location?.OriginalString;
            if (!string.IsNullOrEmpty(currentLocation) && !currentLocation.Contains("Popup", StringComparison.OrdinalIgnoreCase))
            {
                Preferences.Set(LocalStorageKeys.DEFAULT_MODULE, currentLocation);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var savedRoute = Preferences.Get(LocalStorageKeys.DEFAULT_MODULE, string.Empty);
            if (string.IsNullOrEmpty(savedRoute))
            {
                await GoToAsync(defaultRoute);
                return;
            }

            bool routeExists = Items
                .SelectMany(flyoutItem => flyoutItem.Items)
                .SelectMany(tab => tab.Items)
                .Any(content => savedRoute.EndsWith(content.Route));

            if (routeExists)
            {
                try
                {
                    await GoToAsync(savedRoute);
                }
                catch
                {
                    await GoToAsync(defaultRoute);
                }
            }
            else
            {
                await GoToAsync(defaultRoute);
            }
        }
    }
}