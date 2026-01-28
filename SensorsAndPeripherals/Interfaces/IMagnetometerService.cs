using SensorsAndPeripherals.Interfaces.Generic;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IMagnetometerService : ISensorService<MagnetometerChangedEventArgs>
    {
    }
}