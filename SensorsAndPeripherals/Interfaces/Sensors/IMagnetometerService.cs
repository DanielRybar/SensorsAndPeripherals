using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IMagnetometerService : ISensorService<MagnetometerChangedEventArgs>
    {
    }
}