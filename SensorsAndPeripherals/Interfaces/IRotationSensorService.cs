using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IRotationSensorService : ISensorService<OrientationSensorChangedEventArgs>
    {
    }
}