using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class AccelerometerViewModel : SensorViewModel<IAccelerometerService, AccelerometerChangedEventArgs>
    {
        #region variables
        private readonly double multiplier = 15.0;
        private readonly double maxRadius = 125.0;
        private readonly double smoothFactor = 0.2;
        private double lastX = 0.0;
        private double lastY = 0.0;
        #endregion

        #region constructor
        public AccelerometerViewModel()
        {
            DisplayX = $"X: {0:F2} m/s²";
            DisplayY = $"Y: {0:F2} m/s²";
            DisplayZ = $"Z: {0:F2} m/s²";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            // low-pass filter for X and Y
            double currentRawX = e.Reading.Acceleration.X;
            double currentRawY = e.Reading.Acceleration.Y;
            double smoothedX = (currentRawX * smoothFactor) + (lastX * (1 - smoothFactor));
            double smoothedY = (currentRawY * smoothFactor) + (lastY * (1 - smoothFactor));
            lastX = smoothedX;
            lastY = smoothedY;

            // normalize from n * ag to m/s²
            double x = smoothedX * SensorConstants.GRAVITIONAL_ACCELERATION;
            double y = smoothedY * SensorConstants.GRAVITIONAL_ACCELERATION;
            double z = e.Reading.Acceleration.Z * SensorConstants.GRAVITIONAL_ACCELERATION;
            string displayX = $"X: {x:F2} m/s²";
            string displayY = $"Y: {y:F2} m/s²";
            string displayZ = $"Z: {z:F2} m/s²";

            // for better visualization
            double rawX = x * multiplier;
            double rawY = y * multiplier;
            double ballX, ballY;
            double distance = Math.Sqrt((rawX * rawX) + (rawY * rawY));
            if (distance <= maxRadius)
            {
                ballX = rawX;
                ballY = rawY;
            }
            else
            {
                double ratio = maxRadius / distance;
                ballX = rawX * ratio;
                ballY = rawY * ratio;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayX = displayX;
                DisplayY = displayY;
                DisplayZ = displayZ;
                BallX = ballX;
                BallY = ballY;
            });
        }
        #endregion

        #region properties
        protected override SensorSpeed DefaultSpeed => SensorSpeed.Game;

        public double BallX
        {
            get;
            set => SetProperty(ref field, value);
        }

        public double BallY
        {
            get;
            set => SetProperty(ref field, value);
        }
        #endregion
    }
}