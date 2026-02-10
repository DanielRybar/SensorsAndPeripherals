namespace SensorsAndPeripherals.Controls
{
    public class ExtendedEntry : Entry
    {
        public static readonly BindableProperty UnderlineColorProperty = 
            BindableProperty.Create(nameof(UnderlineColor), typeof(Color), typeof(ExtendedEntry), App.Current!.Resources["MainApplicationColor"] as Color);
        public Color UnderlineColor
        {
            get => (Color)GetValue(UnderlineColorProperty);
            set => SetValue(UnderlineColorProperty, value);
        }

        public static readonly BindableProperty CursorColorProperty = 
            BindableProperty.Create(nameof(CursorColor), typeof(Color), typeof(ExtendedEntry), App.Current!.Resources["MainApplicationColor"] as Color);
        public Color CursorColor
        {
            get => (Color)GetValue(CursorColorProperty);
            set => SetValue(CursorColorProperty, value);
        }
    }
}