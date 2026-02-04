using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace SensorsAndPeripherals.Views.Abstract
{
    public abstract partial class ApplicationPage : ContentPage
    {
        private DateTime lastBackButtonPressedTime;

        protected abstract string InfoText { get; }

        // Bindable Property for Loading Overlay
        public static readonly BindableProperty IsWorkingProperty = BindableProperty.Create(
                    propertyName: nameof(IsWorking),
                    returnType: typeof(bool),
                    declaringType: typeof(ApplicationPage),
                    defaultValue: false);

        public bool IsWorking
        {
            get => (bool)GetValue(IsWorkingProperty);
            set => SetValue(IsWorkingProperty, value);
        }

        public ApplicationPage(bool showInfoToolbarItem = true)
        {
            ControlTemplate = new ControlTemplate(() =>
            {
                var mainGrid = new Grid();
                var contentPresenter = new ContentPresenter();
                mainGrid.Add(contentPresenter);
                var loadingOverlay = CreateLoadingOverlay();
                mainGrid.Add(loadingOverlay);
                return mainGrid;
            });

            if (showInfoToolbarItem)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    IconImageSource = App.Current!.Resources["ToolbarHelpIcon"] as FontImageSource,
                    Command = new Command(async () =>
                    {
                        await DisplayAlertAsync("Informace", InfoText, "OK");
                    })
                });
            }
        }

        private Grid CreateLoadingOverlay()
        {
            var vsl = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 5
            };
            var activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                Color = App.Current!.Resources["MainApplicationColor"] as Color
            };
            var loadingLabel = new Label
            {
                Text = App.Current!.Resources["LoadingText"] as string
            };

            vsl.Children.Add(activityIndicator);
            vsl.Children.Add(loadingLabel);

            var overlayGrid = new Grid
            {
                Style = App.Current!.Resources["LoadingLayout"] as Style,
                Children = { vsl }
            };

            overlayGrid.SetBinding(IsVisibleProperty, new Binding(nameof(IsWorking), source: RelativeBindingSource.TemplatedParent));
            return overlayGrid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(App.Current!.Resources["BarColor"] as Color ?? Colors.Black);
        }

        protected override bool OnBackButtonPressed()
        {
            if (Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = false;
                return true;
            }
            if ((DateTime.Now - lastBackButtonPressedTime).TotalSeconds < 2)
            {
                return base.OnBackButtonPressed();
            }
            lastBackButtonPressedTime = DateTime.Now;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Toast.Make("Stiskněte znovu pro ukončení aplikace", ToastDuration.Short).Show();
            });

            return true;
        }
    }
}