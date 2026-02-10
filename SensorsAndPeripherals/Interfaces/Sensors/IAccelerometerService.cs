using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IAccelerometerService : ISensorService<AccelerometerChangedEventArgs>
    {
    }
}