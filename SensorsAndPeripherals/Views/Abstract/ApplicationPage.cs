using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using SensorsAndPeripherals.Helpers;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace SensorsAndPeripherals.Views.Abstract
{
    public abstract partial class ApplicationPage : ContentPage
    {
        private DateTime lastBackButtonPressedTime;
        private readonly Color mainApplicationColor = App.Current?.Resources["MainApplicationColor"] as Color ?? Colors.Black;

        protected abstract string InfoText { get; }
        protected virtual string InfoToolbarItemsText => string.Empty;

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

        // Bindable Property for Loading Text
        public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
            propertyName: nameof(LoadingText),
            returnType: typeof(string),
            declaringType: typeof(ApplicationPage),
            defaultValue: "LoadingDataText".GetStringFromResource());

        public string LoadingText
        {
            get => (string)GetValue(LoadingTextProperty);
            set => SetValue(LoadingTextProperty, value);
        }

        private List<string>? GetNonDefaultToolbarItems()
        {
            var items = ToolbarItems?.Where(item => item.Priority != 1)?.ToList();
            if (items is not null)
            {
                return items.Select(item => item?.IconImageSource is FontImageSource fis ? fis.Glyph : string.Empty).ToList() ?? [];
            }
            return [];
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
                    Priority = 1,
                    Command = new Command(async () =>
                    {
                        var dict = new Dictionary<string, string>();
                        var iconsText = InfoToolbarItemsText.Split(';');
                        var toolbarIcons = GetNonDefaultToolbarItems();
                        if (iconsText?.Length > 0 && iconsText?.Length == toolbarIcons?.Count)
                        {
                            for (int i = 0; i < toolbarIcons?.Count; i++)
                            {
                                dict.Add(toolbarIcons[i], iconsText![i]);
                            }
                        }
                        await PopupExtensions.CreateAndDisplayPopupAsync(InfoText, dict);
                        await Task.Delay(100);
                        Shell.Current.SetAppThemeColor(Shell.BackgroundColorProperty, mainApplicationColor, mainApplicationColor);
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
                Color = mainApplicationColor
            };

            var loadingLabel = new Label();
            loadingLabel.SetBinding(Label.TextProperty, new Binding(nameof(LoadingText), source: RelativeBindingSource.TemplatedParent));

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
            ToolbarFixBug();
        }

        /// <summary>
        /// https://github.com/dotnet/maui/issues/7823#issuecomment-1387094629
        /// </summary>
        private void ToolbarFixBug()
        {
            var toolbarItems = this.ToolbarItems.ToList();
            ToolbarItems.Clear();
            Task.Run(async () =>
            {
                if (toolbarItems?.Count > 0)
                {
                    await Task.Delay(50);
                    Dispatcher.Dispatch(() => toolbarItems.ForEach((toolbar) => this.ToolbarItems.Add(toolbar)));
                }
            });
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
                await Toast.Make("BackButtonExit".GetStringFromResource(), ToastDuration.Short).Show();
            });

            return true;
        }
    }
}