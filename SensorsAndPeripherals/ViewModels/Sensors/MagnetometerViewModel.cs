using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class MagnetometerViewModel : SensorViewModel<IMagnetometerService, MagnetometerChangedEventArgs>
    {
        #region variables
        private readonly string[] directions = ["S", "SV", "V", "JV", "J", "JZ", "Z", "SZ", "S"];
        #endregion

        #region constructor
        public MagnetometerViewModel()
        {
            DisplayX = $"X: {0:F2} µT";
            DisplayY = $"Y: {0:F2} µT";
            DisplayZ = $"Z: {0:F2} µT";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, MagnetometerChangedEventArgs e)
        {
            var data = e.Reading.MagneticField;
            double headingRadians = Math.Atan2(data.Y, data.X);
            double headingDegrees = headingRadians * (180 / Math.PI);
            double heading = headingDegrees - 90;
            if (heading < 0) heading += 360;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Heading = heading;
                DisplayText = $"{heading:F0}° {GetDirectionName(heading)}";
                DisplayX = $"X: {data.X:F2} µT";
                DisplayY = $"Y: {data.Y:F2} µT";
                DisplayZ = $"Z: {data.Z:F2} µT";
            });
        }
        #endregion

        #region methods
        private string GetDirectionName(double degrees)
        {
            var index = (int)Math.Round(degrees % 360 / 45);
            return directions[index];
        }
        #endregion

        #region properties
        public double Heading
        {
            get;
            set => SetProperty(ref field, value);
        }
        #endregion
    }
}