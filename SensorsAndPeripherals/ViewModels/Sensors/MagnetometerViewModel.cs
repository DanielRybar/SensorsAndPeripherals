using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class MagnetometerViewModel : BaseViewModel
    {
        #region variables
        private readonly string[] directions = ["S", "SV", "V", "JV", "J", "JZ", "Z", "SZ", "S"];
        #endregion

        #region services
        private readonly IMagnetometerService magnetometerService = DependencyService.Get<IMagnetometerService>();
        #endregion

        #region constructor
        public MagnetometerViewModel()
        {
            IsSupported = magnetometerService.IsSupported;
            IsMonitoring = magnetometerService.IsMonitoring;
            ToggleCommand = new Command(ToggleSensor, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand ToggleCommand { get; private set; }
        #endregion

        #region event handlers
        private void OnReadingChanged(object? sender, MagnetometerChangedEventArgs e)
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
            magnetometerService.ReadingChanged += OnReadingChanged;
            magnetometerService.Start(SensorSpeed.UI);
            IsMonitoring = true;
        }

        public void StopSensor()
        {
            magnetometerService.Stop();
            magnetometerService.ReadingChanged -= OnReadingChanged;
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
        } = "X: 0,00 µT";

        public string DisplayY
        {
            get;
            set => SetProperty(ref field, value);
        } = "Y: 0,00 µT";

        public string DisplayZ
        {
            get;
            set => SetProperty(ref field, value);
        } = "Z: 0,00 µT";

        public double Heading
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string DisplayText
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;
        #endregion
    }
}