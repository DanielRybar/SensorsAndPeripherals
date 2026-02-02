using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IBarometerService : ISensorService<BarometerChangedEventArgs>
    {
    }
}