namespace SensorsAndPeripherals.Interfaces
{
    public interface IAccelerometerService
    {
        public event EventHandler<AccelerometerChangedEventArgs> ReadingChanged;
        public bool IsMonitoring { get; }
        public bool IsSupported { get; }
        public void Start(SensorSpeed speed);
        public void Stop();
    }
}