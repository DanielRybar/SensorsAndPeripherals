using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IBarometerService : ISensorService<BarometerChangedEventArgs>
    {
    }
}