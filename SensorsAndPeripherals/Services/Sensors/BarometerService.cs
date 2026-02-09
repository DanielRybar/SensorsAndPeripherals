using SensorsAndPeripherals.Interfaces.Sensors;

namespace SensorsAndPeripherals.Services.Sensors
{
    public class BarometerService : IBarometerService
    {
        public event EventHandler<BarometerChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring => Barometer.Default.IsMonitoring;
        public bool IsSupported => Barometer.Default.IsSupported;

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring)
            {
                Barometer.Default.ReadingChanged += OnReadingChanged;
                Barometer.Default.Start(speed);
            }
        }

        public void Stop()
        {
            if (IsSupported && IsMonitoring)
            {
                Barometer.Default.Stop();
                Barometer.Default.ReadingChanged -= OnReadingChanged;
            }
        }

        private void OnReadingChanged(object? sender, BarometerChangedEventArgs e)
        {
            ReadingChanged?.Invoke(this, e);
        }
    }
}