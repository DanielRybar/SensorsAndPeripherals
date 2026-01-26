using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class AccelerometerViewModel : BaseViewModel
    {
        #region variables
        private readonly double multiplier = 15.0;
        private readonly double maxRadius = 125.0;
        private readonly double smoothFactor = 0.2;
        private double lastX = 0.0;
        private double lastY = 0.0;
        #endregion

        #region services
        private readonly IAccelerometerService accelerometerService = DependencyService.Get<IAccelerometerService>();
        #endregion

        #region constructor
        public AccelerometerViewModel()
        {
            IsSupported = accelerometerService.IsSupported;
            IsMonitoring = accelerometerService.IsMonitoring;
            ToggleCommand = new Command(ToggleSensor, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand ToggleCommand { get; private set; }
        #endregion

        #region event handlers
        private void OnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // low-pass filter for X and Y
                double currentRawX = e.Reading.Acceleration.X;
                double currentRawY = e.Reading.Acceleration.Y;
                double smoothedX = (currentRawX * smoothFactor) + (lastX * (1 - smoothFactor));
                double smoothedY = (currentRawY * smoothFactor) + (lastY * (1 - smoothFactor));
                lastX = smoothedX;
                lastY = smoothedY;

                // normalize from n * ag to m/s^2
                var x = smoothedX * SensorConstants.GRAVITIONAL_ACCELERATION;
                var y = smoothedY * SensorConstants.GRAVITIONAL_ACCELERATION;
                var z = e.Reading.Acceleration.Z * SensorConstants.GRAVITIONAL_ACCELERATION;

                DisplayX = $"X: {x:F2} m/s²";
                DisplayY = $"Y: {y:F2} m/s²";
                DisplayZ = $"Z: {z:F2} m/s²";

                // for better visualization
                double rawX = x * multiplier;
                double rawY = y * multiplier;
                double distance = Math.Sqrt((rawX * rawX) + (rawY * rawY));
                if (distance <= maxRadius)
                {
                    BallX = rawX;
                    BallY = rawY;
                }
                else
                {
                    double ratio = maxRadius / distance;
                    BallX = rawX * ratio;
                    BallY = rawY * ratio;
                }
            });
        }
        #endregion

        #region methods
        private void ToggleSensor()
        {
            if (IsMonitoring)
            {
                StopSensor();
            }
            else
            {
                StartSensor();
            }
        }

        private void StartSensor()
        {
            accelerometerService.ReadingChanged += OnReadingChanged;
            accelerometerService.Start(SensorSpeed.Game);
            IsMonitoring = true;
        }

        public void StopSensor()
        {
            accelerometerService.Stop();
            accelerometerService.ReadingChanged -= OnReadingChanged;
            IsMonitoring = false;
        }
        #endregion

        #region properties
        public bool IsSupported
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool IsMonitoring
        {
            get;
            set => SetProperty(ref field, value);
        }

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

        public string DisplayX
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string DisplayY
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string DisplayZ
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;
        #endregion
    }
}