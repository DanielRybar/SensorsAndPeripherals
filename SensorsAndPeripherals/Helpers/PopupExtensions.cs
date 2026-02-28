using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace SensorsAndPeripherals.Helpers
{
    public static class PopupExtensions
    {
        public static async Task CreateAndDisplayPopupAsync(string description, Dictionary<string, string> toolbarItems)
        {
            var lightColor = App.Current?.Resources["PopupLightBackgroundColor"] as Color;
            var darkColor = App.Current?.Resources["PopupDarkBackgroundColor"] as Color;
            var popup = new Popup();
            popup.SetAppThemeColor(Popup.BackgroundColorProperty, lightColor, darkColor);
            popup.Closed += (s, e) =>
            {
                // https://github.com/CommunityToolkit/Maui/issues/2923
                var stealthColor = (App.Current?.Resources["MainApplicationColor"] as Color)!.WithAlpha(0.99f);
                Shell.Current.SetAppThemeColor(Shell.BackgroundColorProperty, stealthColor, stealthColor);
            };

            var grid = new Grid
            {
                Style = App.Current?.Resources["MainGridStyle"] as Style,
                RowDefinitions =
                [
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                ],
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            var closeButton = new Button
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Colors.Transparent,
                ImageSource = App.Current?.Resources["PopupCloseIcon"] as FontImageSource
            };
            closeButton.Clicked += async (s, e) =>
            {
                try
                {
                    await popup.CloseAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while closing the popup: {ex.Message}");
                }
            };
            grid.Add(closeButton);

            var mainVsl = new VerticalStackLayout { Style = App.Current?.Resources["PopupMainVsl"] as Style };
            grid.Add(mainVsl, row: 1);

            var vsl1 = new VerticalStackLayout { Style = App.Current?.Resources["PopupMinorVsl"] as Style };
            var labelDescHeadline = new Label
            {
                Text = "PopupDescriptionLabel".GetStringFromResource(),
                Style = App.Current?.Resources["PopupHeadlineStyle"] as Style
            };
            vsl1.Add(labelDescHeadline);

            var labelDesc = new Label
            {
                Text = description,
                Style = App.Current?.Resources["PopupDescStyle"] as Style
            };
            vsl1.Add(labelDesc);
            mainVsl.Add(vsl1);

            if (toolbarItems.Count > 0)
            {
                var vsl2 = new VerticalStackLayout { Style = App.Current?.Resources["PopupMinorVsl"] as Style };
                var labelIconsHeadline = new Label
                {
                    Text = "PopupIconsLabel".GetStringFromResource(),
                    Style = App.Current?.Resources["PopupHeadlineStyle"] as Style
                };
                vsl2.Add(labelIconsHeadline);

                foreach (var item in toolbarItems)
                {
                    var icon = new Label
                    {
                        Text = item.Key,
                        Style = App.Current?.Resources["PopupIconLabel"] as Style
                    };
                    vsl2.Add(icon);
                    var iconDesc = new Label
                    {
                        Text = item.Value,
                        Style = App.Current?.Resources["CenteredLabel"] as Style
                    };
                    vsl2.Add(iconDesc);
                }
                mainVsl.Add(vsl2);
            }
            popup.Content = grid;

            await Shell.Current.ShowPopupAsync(popup, new PopupOptions()
            {
                Shape = new RoundRectangle()
                {
                    Stroke = Colors.Transparent,
                    CornerRadius = 1
                }
            });
        }
    }
}