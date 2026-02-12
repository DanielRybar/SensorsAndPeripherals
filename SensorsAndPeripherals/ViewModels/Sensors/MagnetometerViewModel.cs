using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class MagnetometerViewModel : SensorViewModel<IMagnetometerService, MagnetometerChangedEventArgs>
    {
        #region variables
        private readonly string[] directions = ["S", "SV", "V", "JV", "J", "JZ", "Z", "SZ", "S"];
        private readonly string microTeslaUnit = "MicroTesla".GetStringFromResource();
        private readonly string degreeUnit = "Degree".GetStringFromResource();
        private double smoothedHeading = 0.0;
        private double smoothedMagX = 0.0;
        private double smoothedMagY = 0.0;
        private double smoothedMagZ = 0.0;
        #endregion

        #region constructor
        public MagnetometerViewModel()
        {
            DisplayX = $"X: {0:F2} {microTeslaUnit}";
            DisplayY = $"Y: {0:F2} {microTeslaUnit}";
            DisplayZ = $"Z: {0:F2} {microTeslaUnit}";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, MagnetometerChangedEventArgs e)
        {
            var data = e.Reading.MagneticField;

            // smoothing values
            smoothedMagX += SensorConstants.SMOOTH_FACTOR * (data.X - smoothedMagX);
            smoothedMagY += SensorConstants.SMOOTH_FACTOR * (data.Y - smoothedMagY);
            smoothedMagZ += SensorConstants.SMOOTH_FACTOR * (data.Z - smoothedMagZ);

            double headingRadians = Math.Atan2(smoothedMagY, smoothedMagX);
            double targetHeading = headingRadians * SensorConstants.RAD_TO_DEG;
            targetHeading -= 90;
            if (targetHeading < 0) targetHeading += 360;

            double delta = targetHeading - smoothedHeading;
            if (delta > 180) delta -= 360;
            else if (delta < -180) delta += 360;
            smoothedHeading += SensorConstants.SMOOTH_FACTOR * delta;
            if (smoothedHeading < 0) smoothedHeading += 360;
            else if (smoothedHeading >= 360) smoothedHeading -= 360;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Heading = smoothedHeading;
                // for better readability, update text only every X ms
                if ((DateTime.Now - lastTextUpdateTime).TotalMilliseconds > SensorConstants.TEXT_VISUALIZATION_INTERVAL_MS)
                {
                    DisplayText = $"{smoothedHeading:F0}{degreeUnit} {GetDirectionName(smoothedHeading)}";
                    DisplayX = $"X: {FormatValue(smoothedMagX, 2, $" {microTeslaUnit}")}";
                    DisplayY = $"Y: {FormatValue(smoothedMagY, 2, $" {microTeslaUnit}")}";
                    DisplayZ = $"Z: {FormatValue(smoothedMagZ, 2, $" {microTeslaUnit}")}";
                    lastTextUpdateTime = DateTime.Now;
                }
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