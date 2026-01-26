using SensorsAndPeripherals.Interfaces;

namespace SensorsAndPeripherals.Services
{
    public class AccelerometerService : IAccelerometerService
    {
        public event EventHandler<AccelerometerChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring => Accelerometer.Default.IsMonitoring;
        public bool IsSupported => Accelerometer.Default.IsSupported;

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring)
            {
                Accelerometer.Default.ReadingChanged += OnReadingChanged;
                Accelerometer.Default.Start(speed);
            }
        }

        public void Stop()
        {
            if (IsSupported && IsMonitoring)
            {
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= OnReadingChanged;
            }
        }

        private void OnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            ReadingChanged?.Invoke(this, e);
        }
    }
}