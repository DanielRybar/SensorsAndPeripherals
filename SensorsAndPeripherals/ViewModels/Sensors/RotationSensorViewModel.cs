using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class RotationSensorViewModel : SensorViewModel<IRotationSensorService, OrientationSensorChangedEventArgs>
    {
        #region variables
        private double smoothedPitch = 0.0;
        private double smoothedRoll = 0.0;
        private double smoothedYaw = 0.0;
        #endregion

        #region constructor
        public RotationSensorViewModel()
        {
            DisplayText = DisplayX = DisplayY = DisplayZ = $"{0:F2}°";
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
            double rollDeg = roll * SensorConstants.RAD_TO_DEG;
            double pitchDeg = pitch * SensorConstants.RAD_TO_DEG;
            double yawDeg = yaw * SensorConstants.RAD_TO_DEG;

            // smoothing angles
            smoothedPitch = SmoothAngle(smoothedPitch, pitchDeg);
            smoothedRoll = SmoothAngle(smoothedRoll, rollDeg);
            smoothedYaw = SmoothAngle(smoothedYaw, yawDeg);
            double totalTilt = Math.Sqrt(Math.Pow(smoothedPitch, 2) + Math.Pow(smoothedRoll, 2));

            MainThread.BeginInvokeOnMainThread(() =>
            {
                RotationX = smoothedPitch;
                RotationY = smoothedRoll;
                RotationZ = smoothedYaw;
                // for better readability, update text only every X ms
                if ((DateTime.Now - lastTextUpdateTime).TotalMilliseconds > SensorConstants.TEXT_VISUALIZATION_INTERVAL_MS)
                {
                    DisplayX = FormatValue(smoothedPitch, 2, "°");
                    DisplayY = FormatValue(smoothedRoll, 2, "°");
                    DisplayZ = FormatValue(smoothedYaw, 2, "°");
                    DisplayText = FormatValue(totalTilt, 2, "°");
                    lastTextUpdateTime = DateTime.Now;
                }
            });
        }
        #endregion

        #region methods
        private static double SmoothAngle(double current, double target)
        {
            double diff = target - current;
            while (diff < -180) diff += 360;
            while (diff > 180) diff -= 360;
            return current + SensorConstants.SMOOTH_FACTOR * diff;
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