using System.Globalization;

namespace SensorsAndPeripherals.Converters
{
    public class FilePathToImageSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string filePath && File.Exists(filePath))
            {
                return ImageSource.FromFile(filePath);
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}