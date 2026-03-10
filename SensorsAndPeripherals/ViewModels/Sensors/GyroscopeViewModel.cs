using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class GyroscopeViewModel : SensorViewModel<IGyroscopeService, GyroscopeChangedEventArgs>
    {
        #region variables
        private readonly double maxRotationSpeed = 200.0;
        private readonly double maxNeedleAngle = 150.0;
        private readonly string unit = "DegreePerSecond".SafeGetResource<string>();
        private double smoothedX = 0.0;
        private double smoothedY = 0.0;
        private double smoothedZ = 0.0;
        #endregion

        #region constructor
        public GyroscopeViewModel()
        {
            DisplayText = DisplayX = DisplayY = DisplayZ = $"{0:F2}{unit}";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, GyroscopeChangedEventArgs e)
        {
            var data = e.Reading.AngularVelocity;
            // rad/s to deg/s
            double rawX = data.X * SensorConstants.RAD_TO_DEG;
            double rawY = data.Y * SensorConstants.RAD_TO_DEG;
            double rawZ = data.Z * SensorConstants.RAD_TO_DEG;

            smoothedX += SensorConstants.SMOOTH_FACTOR * (rawX - smoothedX);
            smoothedY += SensorConstants.SMOOTH_FACTOR * (rawY - smoothedY);
            smoothedZ += SensorConstants.SMOOTH_FACTOR * (rawZ - smoothedZ);
            double totalSpeed = Math.Sqrt(Math.Pow(smoothedX, 2) + Math.Pow(smoothedY, 2) + Math.Pow(smoothedZ, 2));

            double rotX = smoothedX / maxRotationSpeed * maxNeedleAngle;
            double rotY = smoothedY / maxRotationSpeed * maxNeedleAngle;
            double rotZ = smoothedZ / maxRotationSpeed * maxNeedleAngle;
            rotX = Math.Clamp(rotX, -maxNeedleAngle, maxNeedleAngle);
            rotY = Math.Clamp(rotY, -maxNeedleAngle, maxNeedleAngle);
            rotZ = Math.Clamp(rotZ, -maxNeedleAngle, maxNeedleAngle);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                RotationX = rotX;
                RotationY = rotY;
                RotationZ = rotZ;
                // for better readability, update text only every X ms
                if ((DateTime.Now - lastTextUpdateTime).TotalMilliseconds > SensorConstants.TEXT_VISUALIZATION_INTERVAL_MS)
                {
                    DisplayX = FormatValue(smoothedX, 2, unit);
                    DisplayY = FormatValue(smoothedY, 2, unit);
                    DisplayZ = FormatValue(smoothedZ, 2, unit);
                    DisplayText = FormatValue(totalSpeed, 2, unit);
                    lastTextUpdateTime = DateTime.Now;
                }
            });
        }
        #endregion

        #region properties
        public double RotationX
        {
            get;
            set => SetProperty(ref field, value);
        }

        public double RotationY
        {
            get;
            set => SetProperty(ref field, value);
        }

        public double RotationZ
        {
            get;
            set => SetProperty(ref field, value);
        }
        #endregion
    }
}