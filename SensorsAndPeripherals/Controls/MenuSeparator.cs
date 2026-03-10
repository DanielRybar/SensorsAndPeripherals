using SensorsAndPeripherals.Helpers;

namespace SensorsAndPeripherals.Controls
{
    public class MenuSeparator : MenuItem
    {
        public MenuSeparator()
        {
            IsEnabled = false;
            var template = new DataTemplate(() =>
            {
                var label = new Label
                {
                    Style = "MenuSeparator".SafeGetResource<Style>()
                };

                label.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
                return label;
            });
            Shell.SetMenuItemTemplate(this, template);
        }
    }
}