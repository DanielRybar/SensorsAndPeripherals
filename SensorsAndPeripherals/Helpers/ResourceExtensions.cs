namespace SensorsAndPeripherals.Helpers
{
    public static class ResourceExtensions
    {
        public static T SafeGetResource<T>(this string key, T? defaultValue = default)
        {
            if (App.Current?.Resources.TryGetValue(key, out var value) == true && value is T typedValue)
            {
                return typedValue;
            }
            if (defaultValue is not null && !defaultValue.Equals(default(T)))
            {
                return defaultValue;
            }
            return typeof(T) switch
            {
                Type t when t == typeof(string) => (T)(object)key,
                Type t when t == typeof(Color) => (T)(object)Colors.Black,
                Type t when t == typeof(Style) => (T)(object)new Style(typeof(VisualElement)),
                Type t when t == typeof(FontImageSource) => (T)(object)new FontImageSource { Glyph = "?" },
                _ => default!,
            };
        }
    }
}