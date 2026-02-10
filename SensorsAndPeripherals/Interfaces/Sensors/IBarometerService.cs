using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IBarometerService : ISensorService<BarometerChangedEventArgs>
    {
    }
}