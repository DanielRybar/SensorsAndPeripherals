using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using SensorsAndPeripherals.Views.Popups;

namespace SensorsAndPeripherals.Helpers
{
    public static class PopupExtensions
    {
        public static async Task CreateAndDisplayInfoPopupAsync(string[] description, Dictionary<string, string> toolbarItems)
        {
            var popup = new InfoPopup(description, toolbarItems);
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