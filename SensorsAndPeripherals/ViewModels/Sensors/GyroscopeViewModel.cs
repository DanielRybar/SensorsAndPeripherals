using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class GyroscopeViewModel : BaseViewModel
    {
        #region variables
        private readonly double maxRotationSpeed = 200.0;
        private readonly double maxNeedleAngle = 150.0;
        #endregion

        #region services
        private readonly IGyroscopeService gyroscopeService = DependencyService.Get<IGyroscopeService>();
        #endregion

        #region constructor
        public GyroscopeViewModel()
        {
            IsSupported = gyroscopeService.IsSupported;
            IsMonitoring = gyroscopeService.IsMonitoring;
            ToggleCommand = new Command(ToggleSensor, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand ToggleCommand { get; private set; }
        #endregion

        #region event handlers
        private void OnReadingChanged(object? sender, GyroscopeChangedEventArgs e)
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
            gyroscopeService.ReadingChanged += OnReadingChanged;
            gyroscopeService.Start(SensorSpeed.UI);
            IsMonitoring = true;
        }

        public void StopSensor()
        {
            gyroscopeService.Stop();
            gyroscopeService.ReadingChanged -= OnReadingChanged;
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

        public string DisplayX
        {
            get;
            set => SetProperty(ref field, value);
        } = $"{0:F2}°/s";

        public string DisplayY
        {
            get;
            set => SetProperty(ref field, value);
        } = $"{0:F2}°/s";

        public string DisplayZ
        {
            get;
            set => SetProperty(ref field, value);
        } = $"{0:F2}°/s";

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