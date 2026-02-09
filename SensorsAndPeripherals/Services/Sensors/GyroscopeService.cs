using SensorsAndPeripherals.Interfaces.Sensors;

namespace SensorsAndPeripherals.Services.Sensors
{
    public class GyroscopeService : IGyroscopeService
    {
        public event EventHandler<GyroscopeChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring => Gyroscope.Default.IsMonitoring;
        public bool IsSupported => Gyroscope.Default.IsSupported;

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring)
            {
                Gyroscope.Default.ReadingChanged += OnReadingChanged;
                Gyroscope.Default.Start(speed);
            }
        }

        public void Stop()
        {
            if (IsSupported && IsMonitoring)
            {
                Gyroscope.Default.Stop();
                Gyroscope.Default.ReadingChanged -= OnReadingChanged;
            }
        }

        private void OnReadingChanged(object? sender, GyroscopeChangedEventArgs e)
        {
            ReadingChanged?.Invoke(this, e);
        }
    }
}