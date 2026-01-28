namespace SensorsAndPeripherals.Interfaces.Generic
{
    public interface ISensorService<TEventArgs> where TEventArgs : EventArgs
    {
        event EventHandler<TEventArgs> ReadingChanged;
        bool IsMonitoring { get; }
        bool IsSupported { get; }
        void Start(SensorSpeed speed);
        void Stop();
    }
}