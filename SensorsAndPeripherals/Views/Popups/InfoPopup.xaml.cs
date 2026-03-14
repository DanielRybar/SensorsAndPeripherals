using CommunityToolkit.Maui.Views;
using SensorsAndPeripherals.Helpers;
using System.Diagnostics;

namespace SensorsAndPeripherals.Views.Popups;

public partial class InfoPopup : Popup
{
    public InfoPopup(string[] description, Dictionary<string, string> toolbarItems)
    {
        InitializeComponent();
        UpdateLayout(description, new(toolbarItems));
        Closed += (s, e) =>
        {
            // https://github.com/CommunityToolkit/Maui/issues/2923
            var stealthColor = "MainApplicationColor".SafeGetResource<Color>().WithAlpha(0.99f);
            Shell.Current.SetAppThemeColor(Shell.BackgroundColorProperty, stealthColor, stealthColor);
        };
    }

    private void UpdateLayout(string[] description, Dictionary<string, string> toolbarItems)
    {
        if (description.Length > 0)
        {
            ItemDescription.Text = description[0].Trim();
        }
        else
        {
            DescriptionLayout.IsVisible = false;
        }
        if (description.Length > 2)
        {
            ItemValues.Text = description[1].Trim();
            ItemUsage.Text = description[2].Trim();
        }
        else
        {
            ValuesLayout.IsVisible = false;
            UsageLayout.IsVisible = false;
        }
        int toolbarItemsCount = toolbarItems.Count;
        if (toolbarItemsCount > 0)
        {
            IconsLayoutHeadline.Text = toolbarItemsCount >= 2
                ? "PopupMultipleIconsLabel".SafeGetResource<string>()
                : "PopupSingleIconLabel".SafeGetResource<string>();

            foreach (var item in toolbarItems)
            {
                var grid = new Grid
                {
                    ColumnDefinitions = [new() { Width = GridLength.Auto }, new() { Width = GridLength.Star }],
                    ColumnSpacing = 15,
                    Padding = new(0, 10, 0, 0)
                };
                var iconLabel = new Label
                {
                    Text = item.Key,
                    Style = "PopupIconLabel".SafeGetResource<Style>(),
                };
                var descLabel = new Label
                {
                    Text = item.Value,
                    Style = "PopupDescStyle".SafeGetResource<Style>(),
                };
                grid.Add(iconLabel, column: 0);
                grid.Add(descLabel, column: 1);
                IconsLayout.Add(grid);
            }
        }
        else
        {
            IconsLayout.IsVisible = false;
        }
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await CloseAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while closing the popup: {ex.Message}");
        }
    }
}