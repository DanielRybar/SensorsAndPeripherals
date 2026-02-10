using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IGyroscopeService : ISensorService<GyroscopeChangedEventArgs>
    {
    }
}