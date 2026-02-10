using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Sensors
{
    public interface IMagnetometerService : ISensorService<MagnetometerChangedEventArgs>
    {
    }
}