namespace SensorsAndPeripherals.Models.CustomEventArgs
{
    public class LightSensorChangedEventArgs(float illuminance) : EventArgs
    {
        public float Illuminance { get; } = illuminance;
    }
}