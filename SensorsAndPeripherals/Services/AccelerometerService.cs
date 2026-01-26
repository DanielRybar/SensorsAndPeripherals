using SensorsAndPeripherals.Interfaces;

namespace SensorsAndPeripherals.Services
{
    public class AccelerometerService : IAccelerometerService
    {
        public event EventHandler<AccelerometerChangedEventArgs>? ReadingChanged;

        public AccelerometerService()
        {
            if (IsSupported)
            {
                Accelerometer.Default.ReadingChanged += (sender, args) =>
                {
                    ReadingChanged?.Invoke(this, args);
                };
            }
        }

        public bool IsMonitoring => Accelerometer.Default.IsMonitoring;
        public bool IsSupported => Accelerometer.Default.IsSupported;

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring)
            {
                Accelerometer.Default.Start(speed);
            }
        }

        public void Stop()
        {
            if (IsSupported && IsMonitoring)
            {
                Accelerometer.Default.Stop();
            }
        }
    }
}