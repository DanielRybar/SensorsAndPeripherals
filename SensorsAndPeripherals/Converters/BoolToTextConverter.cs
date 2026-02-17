using System.Globalization;

namespace SensorsAndPeripherals.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public string? TextWhenTrue { get; set; }
        public string? TextWhenFalse { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool status)
            {
                return status ? TextWhenTrue : TextWhenFalse;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}