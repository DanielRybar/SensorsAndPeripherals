namespace SensorsAndPeripherals.Interfaces
{
    public interface IMagnetometerService
    {
        public event EventHandler<MagnetometerChangedEventArgs> ReadingChanged;
        public bool IsMonitoring { get; }
        public bool IsSupported { get; }
        public void Start(SensorSpeed speed);
        public void Stop();
    }
}