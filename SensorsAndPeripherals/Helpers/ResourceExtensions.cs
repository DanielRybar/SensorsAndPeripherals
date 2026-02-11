namespace SensorsAndPeripherals.Helpers
{
    public static class ResourceExtensions
    {
        public static string GetStringFromResource(this string key, string defaultValue = "")
        {
            if (App.Current!.Resources.TryGetValue(key, out var value) && value is string valueAsString)
            {
                return valueAsString;
            }
            return defaultValue;
        }
    }
}