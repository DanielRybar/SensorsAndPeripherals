using SensorsAndPeripherals.Interfaces.Generic;
using SensorsAndPeripherals.Models.CustomEventArgs;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IProximitySensorService : ISensorService<ProximitySensorChangedEventArgs>
    {
    }
}