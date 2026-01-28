using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IAccelerometerService : ISensorService<AccelerometerChangedEventArgs>
    {
    }
}