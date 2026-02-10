using SensorsAndPeripherals.Interfaces.Base;
using SensorsAndPeripherals.Models.CustomEventArgs;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface ILightSensorService : ISensorService<LightSensorChangedEventArgs>
    {
    }
}