using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IRotationSensorService : ISensorService<OrientationSensorChangedEventArgs>
    {
    }
}