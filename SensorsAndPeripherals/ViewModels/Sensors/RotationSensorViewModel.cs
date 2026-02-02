using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class RotationSensorViewModel : SensorViewModel<IRotationSensorService, OrientationSensorChangedEventArgs>
    {
        #region variables
        private readonly double radToDeg = 180.0 / Math.PI;
        #endregion

        #region constructor
        public RotationSensorViewModel()
        {
            DisplayX = DisplayY = DisplayZ = $"{0:F2}°";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, OrientationSensorChangedEventArgs e)
        {
            // Quaternion - (x, y, z, w)
            var q = e.Reading.Orientation;

            // Quaternion to Euler angles (roll, pitch, yaw)
            // 1) Roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            double roll = Math.Atan2(sinr_cosp, cosr_cosp);

            // 2) Pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            double pitch;
            if (Math.Abs(sinp) >= 1)
                pitch = Math.CopySign(Math.PI / 2, sinp);
            else
                pitch = Math.Asin(sinp);

            // 3) Yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            double yaw = Math.Atan2(siny_cosp, cosy_cosp);

            // rad to deg
            double rollDeg = roll * radToDeg;
            double pitchDeg = pitch * radToDeg;
            double yawDeg = yaw * radToDeg;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayX = $"{pitchDeg:F2}°";
                DisplayY = $"{rollDeg:F2}°";
                DisplayZ = $"{yawDeg:F2}°";
                RotationX = pitchDeg;
                RotationY = rollDeg;
                RotationZ = yawDeg;
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