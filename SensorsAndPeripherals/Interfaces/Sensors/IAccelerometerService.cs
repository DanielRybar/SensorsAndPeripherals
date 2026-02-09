using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IAccelerometerService : ISensorService<AccelerometerChangedEventArgs>
    {
    }
}