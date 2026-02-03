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
        private double smoothedX = 0.0;
        private double smoothedY = 0.0;
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
            smoothedX = (currentRawX * SensorConstants.SMOOTH_FACTOR) + (this.smoothedX * (1 - SensorConstants.SMOOTH_FACTOR));
            smoothedY = (currentRawY * SensorConstants.SMOOTH_FACTOR) + (this.smoothedY * (1 - SensorConstants.SMOOTH_FACTOR));

            // normalize from n * ag to m/s²
            double x = smoothedX * SensorConstants.GRAVITIONAL_ACCELERATION;
            double y = smoothedY * SensorConstants.GRAVITIONAL_ACCELERATION;
            double z = e.Reading.Acceleration.Z * SensorConstants.GRAVITIONAL_ACCELERATION;

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
                BallX = ballX;
                BallY = ballY;
                // for better readability, update text only every X ms
                if ((DateTime.Now - lastTextUpdateTime).TotalMilliseconds > SensorConstants.TEXT_VISUALIZATION_INTERVAL_MS)
                {
                    DisplayX = $"X: {FormatValue(x, 2, " m/s²")}";
                    DisplayY = $"Y: {FormatValue(y, 2, " m/s²")}";
                    DisplayZ = $"Z: {FormatValue(z, 2, " m/s²")}";
                    lastTextUpdateTime = DateTime.Now;
                }
            });
        }
        #endregion

        #region properties
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