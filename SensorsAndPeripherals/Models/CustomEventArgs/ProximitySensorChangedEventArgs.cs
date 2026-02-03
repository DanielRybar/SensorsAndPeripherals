namespace SensorsAndPeripherals.Models.CustomEventArgs
{
    public class ProximitySensorChangedEventArgs(float distance) : EventArgs
    {
        public float Distance { get; } = distance;
    }
}