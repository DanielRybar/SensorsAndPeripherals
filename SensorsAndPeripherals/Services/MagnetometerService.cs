using SensorsAndPeripherals.Interfaces;

namespace SensorsAndPeripherals.Services
{
    public class MagnetometerService : IMagnetometerService
    {
        public event EventHandler<MagnetometerChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring => Magnetometer.Default.IsMonitoring;
        public bool IsSupported => Magnetometer.Default.IsSupported;

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring)
            {
                Magnetometer.Default.ReadingChanged += OnReadingChanged;
                Magnetometer.Default.Start(speed);
            }
        }

        public void Stop()
        {
            if (IsSupported && IsMonitoring)
            {
                Magnetometer.Default.Stop();
                Magnetometer.Default.ReadingChanged -= OnReadingChanged;
            }
        }

        private void OnReadingChanged(object? sender, MagnetometerChangedEventArgs e)
        {
            ReadingChanged?.Invoke(this, e);
        }
    }
}