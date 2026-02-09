using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IRotationSensorService : ISensorService<OrientationSensorChangedEventArgs>
    {
    }
}