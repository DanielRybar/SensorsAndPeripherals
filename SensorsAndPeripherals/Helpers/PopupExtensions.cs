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
            var lightColor = "PopupLightBackgroundColor".SafeGetResource<Color>();
            var darkColor = "PopupDarkBackgroundColor".SafeGetResource<Color>();
            var popup = new Popup();
            popup.SetAppThemeColor(Popup.BackgroundColorProperty, lightColor, darkColor);
            popup.Closed += (s, e) =>
            {
                // https://github.com/CommunityToolkit/Maui/issues/2923
                var stealthColor = "MainApplicationColor".SafeGetResource<Color>().WithAlpha(0.99f);
                Shell.Current.SetAppThemeColor(Shell.BackgroundColorProperty, stealthColor, stealthColor);
            };

            var grid = new Grid
            {
                Style = "MainGridStyle".SafeGetResource<Style>(),
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
                ImageSource = "PopupCloseIcon".SafeGetResource<FontImageSource>()
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

            var mainVsl = new VerticalStackLayout { Style = "PopupMainVsl".SafeGetResource<Style>() };
            grid.Add(mainVsl, row: 1);

            var vsl1 = new VerticalStackLayout { Style = "PopupMinorVsl".SafeGetResource<Style>() };
            var labelDescHeadline = new Label
            {
                Text = "PopupDescriptionLabel".SafeGetResource<string>(),
                Style = "PopupHeadlineStyle".SafeGetResource<Style>()
            };
            vsl1.Add(labelDescHeadline);

            var labelDesc = new Label
            {
                Text = description,
                Style = "PopupDescStyle".SafeGetResource<Style>()
            };
            vsl1.Add(labelDesc);
            mainVsl.Add(vsl1);

            if (toolbarItems.Count > 0)
            {
                var vsl2 = new VerticalStackLayout { Style = "PopupMinorVsl".SafeGetResource<Style>() };
                var labelIconsHeadline = new Label
                {
                    Text = "PopupIconsLabel".SafeGetResource<string>(),
                    Style = "PopupHeadlineStyle".SafeGetResource<Style>()
                };
                vsl2.Add(labelIconsHeadline);

                foreach (var item in toolbarItems)
                {
                    var icon = new Label
                    {
                        Text = item.Key,
                        Style = "PopupIconLabel".SafeGetResource<Style>()
                    };
                    vsl2.Add(icon);
                    var iconDesc = new Label
                    {
                        Text = item.Value,
                        Style = "CenteredLabel".SafeGetResource<Style>()
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