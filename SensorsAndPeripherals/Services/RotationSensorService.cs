using SensorsAndPeripherals.Interfaces;

namespace SensorsAndPeripherals.Services
{
    public class RotationSensorService : IRotationSensorService
    {
        public event EventHandler<OrientationSensorChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring => OrientationSensor.Default.IsMonitoring;
        public bool IsSupported => OrientationSensor.Default.IsSupported;

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring)
            {
                OrientationSensor.Default.ReadingChanged += OnReadingChanged;
                OrientationSensor.Default.Start(speed);
            }
        }

        public void Stop()
        {
            if (IsSupported && IsMonitoring)
            {
                OrientationSensor.Default.Stop();
                OrientationSensor.Default.ReadingChanged -= OnReadingChanged;
            }
        }

        private void OnReadingChanged(object? sender, OrientationSensorChangedEventArgs e)
        {
            ReadingChanged?.Invoke(this, e);
        }
    }
}