using SensorsAndPeripherals.Interfaces.Generic;
using SensorsAndPeripherals.Models.CustomEventArgs;

namespace SensorsAndPeripherals.Interfaces
{
    public interface ILightSensorService : ISensorService<LightSensorChangedEventArgs>
    {
    }
}