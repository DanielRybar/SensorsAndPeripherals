using CommunityToolkit.Maui.Views;
using SensorsAndPeripherals.Helpers;
using System.Diagnostics;

namespace SensorsAndPeripherals.Views.Popups;

public partial class InfoPopup : Popup
{
    public InfoPopup(string[] description, Dictionary<string, string> toolbarItems)
    {
        InitializeComponent();
        UpdateLayout(description, toolbarItems);
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
        if (toolbarItems.Count > 0)
        {
            foreach (var item in toolbarItems)
            {
                IconsLayout.Add(new Label
                {
                    Text = item.Key,
                    Style = "PopupIconLabel".SafeGetResource<Style>()
                });

                IconsLayout.Add(new Label
                {
                    Text = item.Value,
                    Style = "CenteredLabel".SafeGetResource<Style>()
                });
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