using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class GyroscopeViewModel : SensorViewModel<IGyroscopeService, GyroscopeChangedEventArgs>
    {
        #region variables
        private readonly double maxRotationSpeed = 200.0;
        private readonly double maxNeedleAngle = 150.0;
        #endregion

        #region constructor
        public GyroscopeViewModel()
        {
            DisplayX = DisplayY = DisplayZ = $"{0:F2}°/s";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, GyroscopeChangedEventArgs e)
        {
            var data = e.Reading.AngularVelocity;
            // rad/s to deg/s
            double xDeg = data.X * (180 / Math.PI);
            double yDeg = data.Y * (180 / Math.PI);
            double zDeg = data.Z * (180 / Math.PI);

            string strX = $"{xDeg:F2}°/s";
            string strY = $"{yDeg:F2}°/s";
            string strZ = $"{zDeg:F2}°/s";

            double rotX = xDeg / maxRotationSpeed * maxNeedleAngle;
            double rotY = yDeg / maxRotationSpeed * maxNeedleAngle;
            double rotZ = zDeg / maxRotationSpeed * maxNeedleAngle;
            rotX = Math.Clamp(rotX, -maxNeedleAngle, maxNeedleAngle);
            rotY = Math.Clamp(rotY, -maxNeedleAngle, maxNeedleAngle);
            rotZ = Math.Clamp(rotZ, -maxNeedleAngle, maxNeedleAngle);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayX = strX;
                DisplayY = strY;
                DisplayZ = strZ;
                RotationX = rotX;
                RotationY = rotY;
                RotationZ = rotZ;
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